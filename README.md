
# Bubbles - zadanie rekrutacyjne

Zacznę od tego, że przekroczyłem czas rozwiązania. Całość zajęła ~14.5h.
Jeżeli chcemy liczyć tylko to, co udało się zmieścić w 10h to trzeba wrócić do commita 0982dbc53b82ab6c959ce3f3964bf8215406c191.
W 10h udało mi się zrobić podstawową wersję + 2 punkt (dodawanie nowych kolorów co 50 punktów).
Jednak od początku planowałem zrobić punkt 1 (bo 2 i 3 wydawały się znacznie łatwiejsze), więc reszta kodu była pisana z zamiarem dodania tej funkcjonalności,
dlatego zdecydowałem się dodać to po czasie.

## Design
Zgodnie z zaleceniami całość pisałem tak, żeby przede wszystkim można było wszystko edytować. Wszystkie wartości liczbowe
wypisane w zadaniu może zmieniać w edytorze (chociaż są trochę porozrzucane po całym projekcie) i wszytko powinno działać.
Tak samo starałem się pisać kod, który mógłby być w przyszłości rozszerzalny.
Do optymalizacji nie przykładałem tak bardzo uwagi, ale jednocześnie starałem się nie robić niepotrzebnych operacji. Na przykład
przy sprawdzaniu, czy jest ustawionych 5 lub więcej kulek w jednej linii robie to tylko w miejscu gdzie pojawiła się
nowa kulka, zamiast sprawdzania całego grida. Podobnie używałem pooli dla wszystkiego, więc nie Instantiatuje obiektów
nie potrzebnie, ale pojawia się też sporo abstrakcji, które pewnie można by było uprościć i zwiększyć trochę wydajność
względem elastyczności.

## Domyślne oznaczenia
Nie chciałem bardziej wydłużać czasu, więc w grze nie ma podpowiedzi co jest czym. Grafiki też robione były na szybko, więc może być 
ciężko się zorientować na pierwszy rzut oka co jest czym. Krótki spis:
- kulka z białą gwiazdką - kulka posiada umiejętność
- kulka eksplozji - czerwona
- kulka pamięci - niebieska
  - pomnik - czarny trapez
- kulka szczęścia - zielona
  - kulka tęczowa - tęcza
- kulka problemu - żółta
  - kulki zablokowane mają czarną obwódkę
- kulka chochlik - magenta
  - chochlik - filotetowy stworek (lepiej się nie przyglądać xD)
  - następny ruch chochlika - fioletowy romb
- kulka żniwaiarz - szara
  - w tym przypadku trzeba najbardziej uważać, bo nie ma specjalnego
  oznaczenia i po uruchomieniu  umiejętności wybieramy cel (lub kilka) do usunięcia 

## Edge cases
Sporo się tego pojawia. Szczególnie to widać jak uruchomimy największą mapę, spawn 10 nowych kulek co turę, każda z nich
będzie miała 100% szansy na umiejętność i potrzeba 3 w linii do zbicia (przy takiej konfiguracji przede wszystkim trzeba uważać 
na żniwiarzy, bo strzały się stackują, a często możemy nawet nie zobaczyć, że kulka jest zbijana).
Jednak starałem się, żeby wszystko było intuicyjne i działało tak jak byśmy się tego spodziewali. Przykładowe decyzje:

- Chochlika nie można blokować - w zadaniu nie było określone co ma się stać, gdy gracz próbowałby zablokować kolejną
  pozycje chochlika. Zdecydowałem się rozwiązać to tak, że przyszła lokalizacja chochlika jest zablokowana.
  Była to opcja najłatwiejsza dla mnie do zaprogramowania i też ma sens gameplayowo.

- Spawn kulek / koniec gry - można to zrobić przynajmniej na dwa sposoby. Pierwszy (który ja wykorzystałem) zakłada, że wszystkie 3 kulki
  pojawiają się na mapie jednocześnie i dopiero po tym sprawdzamy, czy tworzą one jakąś linie. W drugim podejściu po pojedynczym
  spawnie sprawdzamy, czy tworzy on jakąś linie. Odpowiedź może wydawać się oczywista, aby wybrać pierwszą, bo kulki nie mają
  określonej kolejności. Istotna różnica pojawia się przy końcu gry, bo stosując pierwsze rozwiązanie uznajemy przegraną gdy nie
  ma miejsca na spawn 3 nowych kulek, co nie do końca pokrywa się z poleceniem.
  Oznacza to również, że przed wyświetleniem końcowego ekranu powinniśmy dopełnić puste miejsca kulkami.
  Może to spowodować sytuacje, że wizualnie na ekranie pojawią się kulki tworząc linie 5 co może zmylić użytkownika.
  Przy drugim podejściu w takiej sytuacji nastąpiłoby zbicie i gracz mógłby kontynuować grę. Ja mimo wszystko wybrałem pierwsze
  podejście i pewnie zdecydowana większość użytkowników nigdy by nie zauważyła różnicy :D
