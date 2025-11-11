[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/ariel-gallardo/clean-code)
docker run -it --rm --name rabbitmq-local -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin rabbitmq:4-management
docker run -it --rm --name redis-local -p 6379:6379 -e REDIS_PASSWORD=admin redis:latest redis-server --requirepass admin