#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
TEST_COMPOSE_FILE="${ROOT_DIR}/docker-compose.psql.test.yml"
TEST_PROJECT_NAME="${TEST_PROJECT_NAME:-ddon-test}"

SOURCE_DB_CONTAINER="${SOURCE_DB_CONTAINER:-ddon-db}"
TARGET_DB_CONTAINER="${TARGET_DB_CONTAINER:-ddon-db-test}"
TARGET_APP_CONTAINER="${TARGET_APP_CONTAINER:-ddon-server-test}"
DB_NAME="${DB_NAME:-postgres}"
DB_USER="${DB_USER:-postgres}"
DB_PASS="${DB_PASS:-postgres}"

if docker compose version >/dev/null 2>&1; then
  COMPOSE_CMD=(docker compose)
elif command -v docker-compose >/dev/null 2>&1; then
  COMPOSE_CMD=(docker-compose)
else
  echo "Error: neither 'docker compose' nor 'docker-compose' is available."
  exit 1
fi

echo "Cleaning stale test stack resources..."
"${COMPOSE_CMD[@]}" -p "${TEST_PROJECT_NAME}" -f "${TEST_COMPOSE_FILE}" down --remove-orphans >/dev/null 2>&1 || true

# Remove stale named containers that can hold references to deleted networks.
docker rm -f "${TARGET_APP_CONTAINER}" "${TARGET_DB_CONTAINER}" >/dev/null 2>&1 || true

if ! docker ps --format '{{.Names}}' | grep -Fxq "${SOURCE_DB_CONTAINER}"; then
  echo "Error: source database container '${SOURCE_DB_CONTAINER}' is not running."
  echo "Start your active PostgreSQL stack first (expected DB container: ddon-db)."
  exit 1
fi

echo "Starting isolated test database container..."
"${COMPOSE_CMD[@]}" -p "${TEST_PROJECT_NAME}" -f "${TEST_COMPOSE_FILE}" up -d db

echo "Waiting for test database to become ready..."
for _ in {1..60}; do
  if docker exec "${TARGET_DB_CONTAINER}" pg_isready -U "${DB_USER}" -d "${DB_NAME}" >/dev/null 2>&1; then
    break
  fi
  sleep 1
done

if ! docker exec "${TARGET_DB_CONTAINER}" pg_isready -U "${DB_USER}" -d "${DB_NAME}" >/dev/null 2>&1; then
  echo "Error: test database '${TARGET_DB_CONTAINER}' did not become ready in time."
  exit 1
fi

echo "Cloning database '${DB_NAME}' from '${SOURCE_DB_CONTAINER}' to '${TARGET_DB_CONTAINER}'..."
docker exec -e PGPASSWORD="${DB_PASS}" "${SOURCE_DB_CONTAINER}" \
  pg_dump -U "${DB_USER}" -d "${DB_NAME}" --clean --if-exists --no-owner --no-privileges \
  | docker exec -i -e PGPASSWORD="${DB_PASS}" "${TARGET_DB_CONTAINER}" \
  psql -U "${DB_USER}" -d "${DB_NAME}"

echo "Clearing transient session rows from cloned test database..."
docker exec -e PGPASSWORD="${DB_PASS}" "${TARGET_DB_CONTAINER}" \
  psql -U "${DB_USER}" -d "${DB_NAME}" -c 'TRUNCATE TABLE "ddon_connection";'

echo "Starting isolated test app container..."
"${COMPOSE_CMD[@]}" -p "${TEST_PROJECT_NAME}" -f "${TEST_COMPOSE_FILE}" up -d --build app

echo "Verifying test app container is running..."
for _ in {1..20}; do
  if docker ps --format '{{.Names}}' | grep -Fxq "${TARGET_APP_CONTAINER}"; then
    break
  fi
  sleep 1
done

if ! docker ps --format '{{.Names}}' | grep -Fxq "${TARGET_APP_CONTAINER}"; then
  echo "Error: test app container '${TARGET_APP_CONTAINER}' is not running."
  echo "Recent logs from '${TARGET_APP_CONTAINER}':"
  docker logs --tail 120 "${TARGET_APP_CONTAINER}" || true
  echo
  echo "Container states for project '${TEST_PROJECT_NAME}':"
  "${COMPOSE_CMD[@]}" -p "${TEST_PROJECT_NAME}" -f "${TEST_COMPOSE_FILE}" ps || true
  exit 1
fi

echo
echo "Test server is up with cloned DB state."
echo "Mapped ports: login=53100, game=53000, web=53099, postgresql=5433"
echo "Stop with: ${COMPOSE_CMD[*]} -p ${TEST_PROJECT_NAME} -f ${TEST_COMPOSE_FILE} down"