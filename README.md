# Maze-Solver

Aplikacja mobilna na system Android przetwarzająca zdjęcie/obrazek z labiryntem, a następnie wyszukująca w nim
wyjście.

# Źródła labiryntu

Aplikacja umożliwia przetwarzanie
* zdjęć przechwytywanych z aparatu
* wcześniej wykonanych zdjęć z pamięci telefonu
* obrazków z pamięci telefonu

# Konwersja obrazu na labirynt

### Progowanie

Pierwszym etapem konwersji jest wyróżnienie tych pikseli ze zdjęcia, które są najbardziej zbliżone do koloru
czarnego. Użytkownik może wpływać na granicę progowania za pośrednictwem suwaka.

### Regeneracja obrazu

Kolejnym etapem jest użycie zmodyfikowanego algorytmu automatu komórkowego, który uzupełnia braki w obrazie na
podstawie sąsiadujących komórek. Ułatwia to detekcje linii oraz kształtów w następnym kroku. Użytkownik może
wpłynąć na liczbę iteracji algorytmu.

### Usuwanie zakłóceń

Następnym etapem jest usuwanie zakłóceń między innymi w postaci cienia występujących na zdjęciach wykonanych
aparatem. W procesie tym rozpoznaje się kształty składające się z aktywnych pikseli i usuwa te, które sąsiadują z krawędzią oraz których powierzchnia jest mniejsza od ustalonego progu.

### Detekcja linii

W tym etapie na podstawie obrazu rozpoznawane są poziome i pionowe linie, które tworzą labirynt. Użytkownik poprzez zmianę parametrów grubości linii oraz odstępu między nimi ma wpływ na kształt nowego labiryntu.

### Składanie labiryntu

Ostatnim etapem jest odtworzenie labiryntu na podstawie zebranych linii. Na podstawie położenia punktów tworzących linie wyznaczane są miejsca skrzyżowań labiryntu.

# Rozwiązanie labiryntu

Jeśli stworzony labirynt jest poprawny:
* ma prostokątny kształt
* zawiera dokładnie 2 punkty wejścia
* jest szeroki i wysoki na co najmniej 3 jednostki
można przejść do kolejnego kroku, którym jest wyszukiwanie ścieżki.

W tym kroku wyszukiwana jest droga z punktu A do punktu B. Jeśli takowa istnieje, zostaje ona nakreślona.

# Informacje


Aplikacja została stworzona na system Android w technologii Xamarin w języku C# przez TukanHan.