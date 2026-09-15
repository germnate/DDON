#!/bin/sh
set -eu

: "${BAZAAR_ROTATION_SCHEDULE:=0 */6 * * *}"

cat >/etc/cron.d/ddon-bazaar <<EOF
${BAZAAR_ROTATION_SCHEDULE} root DDON_DISABLE_ASSET_WATCHERS=true /var/ddon/server/Arrowgene.Ddon.Cli bazaar rotate --config=Files/Arrowgene.Ddon.config.json >> /var/log/ddon-bazaar.log 2>&1
EOF

chmod 0644 /etc/cron.d/ddon-bazaar
touch /var/log/ddon-bazaar.log

exec cron -f