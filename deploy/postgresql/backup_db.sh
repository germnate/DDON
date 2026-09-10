#!/usr/bin/env bash
#
# Creates a compressed pg_dump backup of the DDON PostgreSQL database running in Docker.
#
# Defaults match docker-compose.psql.yml (container 'ddon-db', database 'postgres').
# Dumps are written in pg_dump custom format (-Fc) so they can be restored with
# ./import_pg_dump.sh or directly via pg_restore.
#
# Usage:
#   ./backup_db.sh [options]
#
# Options:
#   -c, --container NAME   Docker container running PostgreSQL   (default: ddon-db)
#   -d, --database NAME    Database to dump                      (default: postgres)
#   -u, --user NAME        PostgreSQL user                       (default: postgres)
#   -o, --output-dir DIR   Directory to write the dump into      (default: <repo>/backups)
#   -k, --keep N           Keep only the N most recent dumps     (default: 0 = keep all)
#   -p, --plain            Write a plain .sql script instead of custom format
#   -h, --help             Show this help text
#
# Environment:
#   DB_PASS   Password for the PostgreSQL user (default: postgres)
#
# Examples:
#   ./backup_db.sh
#   ./backup_db.sh --output-dir /mnt/backups --keep 7
#   DB_PASS=secret ./backup_db.sh --container ddon-db-test --database postgres

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

DB_CONTAINER="${DB_CONTAINER:-ddon-db}"
DB_NAME="${DB_NAME:-postgres}"
DB_USER="${DB_USER:-postgres}"
DB_PASS="${DB_PASS:-postgres}"
BACKUP_DIR="${BACKUP_DIR:-${ROOT_DIR}/backups}"
KEEP=0
PLAIN=false

usage() {
  # Print the leading comment block (everything after the shebang up to the
  # first non-comment line) with the leading '#' markers stripped.
  awk 'NR == 1 { next } /^#/ { sub(/^# ?/, ""); print; next } { exit }' "${BASH_SOURCE[0]}"
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    -c|--container)  DB_CONTAINER="$2"; shift 2 ;;
    -d|--database)   DB_NAME="$2"; shift 2 ;;
    -u|--user)       DB_USER="$2"; shift 2 ;;
    -o|--output-dir) BACKUP_DIR="$2"; shift 2 ;;
    -k|--keep)       KEEP="$2"; shift 2 ;;
    -p|--plain)      PLAIN=true; shift ;;
    -h|--help)       usage; exit 0 ;;
    *) echo "Error: unknown option '$1'. Use --help for usage." >&2; exit 1 ;;
  esac
done

if ! [[ "${KEEP}" =~ ^[0-9]+$ ]]; then
  echo "Error: --keep expects a non-negative integer, got '${KEEP}'." >&2
  exit 1
fi

if ! command -v docker >/dev/null 2>&1; then
  echo "Error: 'docker' is not available on PATH." >&2
  exit 1
fi

if ! docker ps --format '{{.Names}}' | grep -Fxq "${DB_CONTAINER}"; then
  echo "Error: database container '${DB_CONTAINER}' is not running." >&2
  echo "Start the PostgreSQL stack first: docker compose -f docker-compose.psql.yml up -d db" >&2
  exit 1
fi

if ! docker exec "${DB_CONTAINER}" pg_isready -U "${DB_USER}" -d "${DB_NAME}" >/dev/null 2>&1; then
  echo "Error: database '${DB_NAME}' in '${DB_CONTAINER}' is not accepting connections." >&2
  exit 1
fi

mkdir -p "${BACKUP_DIR}"

TIMESTAMP="$(date +"%Y%m%d_%H%M%S")"
if [[ "${PLAIN}" == true ]]; then
  EXTENSION="sql"
  FORMAT_ARGS=(--format=plain)
else
  EXTENSION="dump"
  FORMAT_ARGS=(--format=custom --compress=9)
fi
OUTPUT_FILE="${BACKUP_DIR}/${DB_NAME}_${TIMESTAMP}.${EXTENSION}"

echo "Dumping '${DB_NAME}' from container '${DB_CONTAINER}'..."
echo "Destination: ${OUTPUT_FILE}"

# Remove a partial dump if the process is interrupted or pg_dump fails.
cleanup_partial() {
  if [[ -f "${OUTPUT_FILE}" ]]; then
    rm -f "${OUTPUT_FILE}"
    echo "Removed incomplete dump '${OUTPUT_FILE}'." >&2
  fi
}
trap cleanup_partial ERR INT TERM

# PGPASSWORD is passed through docker exec so the password never lands on disk
# inside the container and never appears in the container's process arguments.
docker exec -e PGPASSWORD="${DB_PASS}" "${DB_CONTAINER}" \
  pg_dump -U "${DB_USER}" -d "${DB_NAME}" \
  "${FORMAT_ARGS[@]}" --no-owner --no-privileges \
  > "${OUTPUT_FILE}"

trap - ERR INT TERM

if [[ ! -s "${OUTPUT_FILE}" ]]; then
  echo "Error: dump file '${OUTPUT_FILE}' is empty." >&2
  rm -f "${OUTPUT_FILE}"
  exit 1
fi

if [[ "${PLAIN}" == false ]]; then
  echo "Verifying dump integrity..."
  if ! docker exec -i "${DB_CONTAINER}" pg_restore --list < "${OUTPUT_FILE}" >/dev/null 2>&1; then
    echo "Error: '${OUTPUT_FILE}' is not a readable pg_dump archive." >&2
    rm -f "${OUTPUT_FILE}"
    exit 1
  fi
fi

SIZE="$(du -h "${OUTPUT_FILE}" | cut -f1)"
echo "Backup completed successfully (${SIZE})."

if [[ "${KEEP}" -gt 0 ]]; then
  echo "Pruning old backups, keeping the ${KEEP} most recent..."
  # Filenames embed a zero-padded timestamp, so a reverse lexical sort is
  # equivalent to sorting newest-first and avoids depending on GNU find.
  while IFS= read -r OLD; do
    [[ -n "${OLD}" ]] || continue
    rm -f "${OLD}"
    echo "  removed $(basename "${OLD}")"
  done < <(
    find "${BACKUP_DIR}" -maxdepth 1 -type f -name "${DB_NAME}_*.${EXTENSION}" \
      | sort -r \
      | tail -n "+$((KEEP + 1))"
  )
fi

echo
echo "Restore with:"
if [[ "${PLAIN}" == true ]]; then
  echo "  docker exec -i -e PGPASSWORD=\"\${DB_PASS}\" ${DB_CONTAINER} psql -U ${DB_USER} -d ${DB_NAME} < ${OUTPUT_FILE}"
else
  echo "  ./import_pg_dump.sh ${DB_CONTAINER} ${DB_NAME} ${DB_USER} \"\${DB_PASS}\" ${OUTPUT_FILE}"
fi
