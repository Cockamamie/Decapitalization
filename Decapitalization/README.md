Результаты работы в `/Test/Output`

Правила капитализации
* Буква после `. ` или `.\n`
* Буква после `? ` или `?\n`
* Буква после `! ` или `!\n`
* Буква после `\n\n`
* Буква после кавычек
* Буква после двух подряд или более заглавных
* Первые 2 символа текста всегда являются отклонением, если они заглавные(упрощение реализации)

или регуляркой: `[.?!][ \n]|(.|\n)“|[A-Z]{2}|\n\n`

Из интересного насчет результата(если код написан верно): в промежутке `[0...254]` присутствуют все числа, т.е.\
использование для сжатия алгоритма Хаффмана или подобного, основанного\
на частоте, не имеет смысла.\
А еще указанные правила для данного текста отбросили лишь примерно половину\
заглавных букв, что печально и напрягает.

Итого по памяти вышло 5 КБ информации на 400 КБ текста

Память на хранение контекстных моделей:
Посчитаем по результам работы программы по формуле\
`x байт = 3 * кол-во моделей + 3 * суммарно символов по всем моделям(байт на символ, 2 байта на кол-во вхождений)`

Итого вышло ~110 КБ. В теории используя кодирование с переполнением для кол-ва вхождений можно было бы уложиться в ~70 КБ