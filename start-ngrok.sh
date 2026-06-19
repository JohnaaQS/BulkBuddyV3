#!/bin/bash
# BulkBuddy – ngrok tunnel naar localhost:5187
# Vereist: ngrok geinstalleerd (brew install ngrok)

echo "Start ngrok tunnel op poort 5187..."
echo "Zorg dat de app draait: dotnet run --project BulkBuddy.Web"
echo ""
ngrok http 5187
