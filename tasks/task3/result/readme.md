# Описание

В booking-subgraph внесено обращение к booking-service с помощью gRPC, а также проверка хэдера userId. В gateway добавлена пересылка заголовков оригинального реквеста в сервисы. В hotel-subgraph добавлены обращения в API hotel-monolith.

Полезных логов booking-subgraph и gateway не пишут при отправке запросов.

Для запуска требуются поднятые проекты из Task2. Чтобы запустить, выполните `docker compose up -d --build`.
