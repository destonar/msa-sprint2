#!/bin/bash

set -e

echo "▶️ Проверка Feature Flag (X-KafkaPublish-Enabled: true)..."

# Отправляем запрос с заголовком, чтобы маршрутизировать трафик на `v2`
curl -H "X-KafkaPublish-Enabled: true" http://localhost:9090/ping
