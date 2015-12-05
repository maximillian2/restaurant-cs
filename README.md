# Курсовий проект на тему _"Замовлення страв в ресторані"_

## Функціональні вимоги:
1. Управління інгредієнтами
  - [ ] Можливість додавати інгредієнт
  - [ ] Можливість видаляти інгредієнг (у випадку що він не входить до складу якоїсь
страви)
  - [ ] Можливість змінити інгредієнт
  - [ ] Можливість переглянути перелік всіх інгредієнтів
2. Управління стравами
  - [ ] Можливість додавати страву
  - [ ] Можливість видаляти страву
  - [ ] Можливість змінювати страву
      - [ ] Можливість додавати та видаляти інгредієнти страви
      - [ ] Можливість змінювати назву страви
      - [ ] Можливість змінювати ціну страви
      - [ ] Можливість змінювати час приготування страви
  - [ ] Можливість переглянути інформацію про конкретну страву
3. Управління замовленнями
  - [ ] Можливість додати замовлення
  - [ ] Можливість видалиги замовлення
  - [ ] Можливість змінити замовлення
      - [ ] змінити кількість страв
      - [ ] змінити загальну вартість
      - [ ] змінити номер столика даного замовлення
 - [ ] Можливість переглянути інформацію про замовлення


## Нефункціональні вимоги:

1. Вимоги до зовнішнього інтерфейсу
  - Зовнішній інтерфейс користувача має бути командним
  - Застосування має бути консольним
2. Дані повинні зберігатись у файлах після виходу з програми
3. Система має забезпечити правильність введених даних та коректну обробку виключних
ситуацій
4. Логічна структура даних
  - Інформаційний об'єкт страва

Елемент даних | Тип | Опис 
---|---|---
Назва | String | Назва страви 
Інгредієнти | List | Перелік інгредієнтів конкретної страви
Ціна | Float | Ціна конретної страви
Час | Float | Час приготування конретної страви

  4.2 Інформаційний об'єкт замовлення

Елемент даних | Тип | Опис 
---|---|---
Страви | List | Перелік страв у конретному замовленні
Загальна вартість | Integer | Загальна вартість конкретного замовлення
Номер столика | Integer | Номер столика, на який повинні принести конретне замовлення
