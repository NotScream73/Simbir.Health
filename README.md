# Основное задание:
1. Account URL: http://localhost:5000/swagger
2. Hospital URL: http://localhost:5001/swagger
3. Timetable URL: http://localhost:5002/swagger
4. Document URL: http://localhost:5003/swagger

Перед запуском необходимо убедиться, что порты 5000, 5001, 5002, 5003, 5433, 5434, 5435, 5436 свободны.

# Дополнительная информация которую вы захотите указать
В Account microservice добавил GET /api/Accounts/{id}/Roles для получения ролей пользователя по id пользователя.
Метод доступен авторизованным пользователям с одной из следующиъ ролей: Admin, Manager, Doctor.
Возвращается массив названий ролей пользователя.
