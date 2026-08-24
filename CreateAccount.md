curl -sS -X POST http://localhost:52099/api/account \
  -H "Content-Type: application/json" \
  --data '{"Action":"create","Account":"nate","Email":"nate@example.com","Password":"nate"}'