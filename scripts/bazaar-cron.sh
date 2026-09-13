#!/bin/sh
set -eu

: "${BAZAAR_ROTATION_SCHEDULE:=0 0 * * *}"

cat >/etc/cron.d/ddon-bazaar <<EOF
${BAZAAR_ROTATION_SCHEDULE} root dotnet /var/ddon/server/Arrowgene.Ddon.Cli.dll bazaar rotate --config=Files/Arrowgene.Ddon.config.json >> /var/log/ddon-bazaar.log 2>&1
EOF

chmod 0644 /etc/cron.d/ddon-bazaar
touch /var/log/ddon-bazaar.log

exec cron -f