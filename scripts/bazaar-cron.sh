#!/bin/sh
set -eu

: "${BAZAAR_ROTATION_SCHEDULE:=0 */6 * * *}"

cat >/etc/cron.d/ddon-bazaar <<EOF
SHELL=/bin/sh
PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
TZ=${TZ:-America/New_York}
DB_TYPE=${DB_TYPE:-}
DB_DATABASE=${DB_DATABASE:-}
DB_FOLDER=${DB_FOLDER:-}
DB_HOST=${DB_HOST:-}
DB_USER=${DB_USER:-}
DB_PASS=${DB_PASS:-}
DB_PORT=${DB_PORT:-}
DB_WIPE_ON_STARTUP=${DB_WIPE_ON_STARTUP:-}
DB_BUFFER_SIZE=${DB_BUFFER_SIZE:-}
DB_NO_RESET_ON_CLOSE=${DB_NO_RESET_ON_CLOSE:-}
DB_ENABLE_TRACING=${DB_ENABLE_TRACING:-}
DB_ENABLE_POOLING=${DB_ENABLE_POOLING:-}
DB_MAX_AUTO_PREPARE=${DB_MAX_AUTO_PREPARE:-}

${BAZAAR_ROTATION_SCHEDULE} root cd /var/ddon/server && DDON_DISABLE_ASSET_WATCHERS=true /var/ddon/server/Arrowgene.Ddon.Cli bazaar rotate --config=/var/ddon/server/Files/Arrowgene.Ddon.config.json >> /proc/1/fd/1 2>&1
EOF

chmod 0644 /etc/cron.d/ddon-bazaar

echo "Installed DDON bazaar cron schedule: ${BAZAAR_ROTATION_SCHEDULE}"
echo "Cron job output will be written to the container log."

exec cron -f
