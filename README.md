# A-Pathfinding_dansk
Her er det mit mål at fremlægge brugen af og forklaring af A* Pathfinding

Tags: #unity #CSharp 
Ekstern Dokumentation:
link 1: https://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
link 2: https://www.kodeco.com/3016-introduction-to-a-pathfinding

Jeg har forsøgt med udgangspunkt i de 2 ovenstående artikler at lave mit eget.


## A* Pathfinding - hvad er det?
Et af de mest populære pathfinding metode i unity hedder *A * pathfinding*  

Metoden går ud af at  [[Algorithm]]en skal finde  den korteste rejse mellem to punkter **A**
og **B**.  I den øverste artikel har forfatteren introduceret nogle forhindringer for at gøre det svære for algoritmen at finde vej. Dette er gjort med et 'X'.

```C#
string[] map = new string[]

{

    "+------+",
    "|      |",
    "|A X   |",
    "|XXX   |",
    "|   X  |",
    "| B    |",
    "|      |",
    "+------+",

};
```

[[Algorithm]]en har brug for at holde styr på hvilke 'tiles' den allerede har besøgt (closed list), og den som den overvejer at besøge(open list).

For at gøre dette er der brug for følgende klasse:
```C#
private Class Location
{
	public int X;
	public int Y;
	public int F;
	public int G;
	public int H;
	public int Location Parent;
}
```


### Mere om Open og Closed Lists
Der er brug for to liste for at dette skal kunne lade sig gøre, en *open* og en *closed*. 
1. **Open List:** denne liste har alle de 'tiles' som der betragtes som at være en mulighed for korteste rute.
2. **Closed List:** Denne liste har alle de 'tiles' som ikke behøver at blive betragtet som kandidat igen.


## Rute Scoring
Vores 'tile' vil have koordinaterne X og Y til rådighed og hver 'tile' vil få 3 scoringer
1. F - Er G + H
2. G - Er distancen fra *Udgangspunktet* 
3. H - Er distancen fra *Destinationen* 

### Mere om G 

**G** er *Movement Cost* fra *Udgangspunktet* til det nuværende 'tile'. Så for en 'tile' som er lige ved siden af *Udgangspunktet* vil denne få værdi 1, men dette vil stige jo længere væk vi kommer.

For at kunne udregne G er vi nød til at tage G fra dens *Parent* (Den 'tile' vi kom fra) og ligge 1 til denne. Derfor vil G i hvert 'tile' repræsentere den totale *cost* for den genererede rute fra *udgangspunktet* til *destinationen*. Billedet nedenfor viser hvordan 2 forskellige ruter har forskellige værdier på hvert 'tile'
![[Pasted image 20230801131602.png]]

### Mere om H 
**H** er den estimerede *Movement Cost* fra det nuværende 'tile' til *Destinationen*. Denne bliver ofte kaldt for *heuristisk*, vi vil ikke kende den virkelige *cost* endnu - blåt estimeringen.

Jo tættere den estimerede *Movement Cost* er på den faktiske *Cost*, jo mere akkurat er den endelige rute. 
Til dette benyttes *Manhattan Distance Method* også kaldet *Manhattan Length*/*City Block Distance*. Denne bruges til at tælle antallet af horisontale og vertikale 'tiles' der mangler for at nå *destinationen*, uden at tage udfordringer eller andre features ind i sammenhængen.
Følgende billede viser **H**/ *City Block Distance* tallet i sort som er afstanden fra katten til knoglen.
![[Pasted image 20230801132018.png]]

At udregne **H** ser sådan ud:
```C#
static int ComputeHScore(int x, int y, int targetX, int targetY)
{
	return Math.Abs(targetX - x) + Math.Abs(targetY - y);
}
```

## A* Algoritme

Nu hvor vi har styr på at udregne værdien for hvert 'tile' kan vi se nærmere på hvordan [[Algorithm]]en virker.

Vores Walker vil finde den korteste rute ved at gøre følgende:
1. finde det 'tile' på *Open List* som har den laveste værdi, denne kaldes 'S'.
2. Fjerne 'S' fra *Open List* og tilføj 'S' til *Closed List*
3. for hvert 'tile' **T** i 'S'  tilstødende 'tiles'
	1. hvis **T** er på *Closed List* ignoreres denne
	2. hvis **T** ikke er på *Open List*, tilføjes denne og dens score udregnes
	3. hvis **T** allerede er på *Open List*: Check at F scoren er lavere end den nuværende rute til at nå denne, hvis det er, opdater den og dens *parents* score.
### Eksempel
med udgangspunkt i dette billede:
![[Pasted image 20230801133128.png]]

I dette diagram er følgende score opsat på følgende måde:
* **F**  Score for dette ' Grøn tile' - øverste venstre hjørne
* **G** *cost* for at nå fra kat til det tilstødende ' Grøn tile' - nederste venstre hjørne
* **H**  Estimeret *cost* for at nå fra 'Grøn tile' til *Destination* - 

Pilene viser retning

Katten vil gå efter den laveste **F** værdi og til sidst vil ruten se sådan ud:
![[Pasted image 20230801133808.png]]

## Nu til Kodning!
Som nævnt før har vi følgende ting med:
Nogle [[variable]]r, disse skal bruges som en del af den [[class]] vi kalder "Location".
```C#
private Class Location
{
	public int X;
	public int Y;
	public int F;
	public int G;
	public int H;
	public int Location Parent;
}
```

og udregningen af **H**
```C#
static int ComputeHScore(int x, int y, int targetX, int targetY)
{
	return Math.Abs(targetX - x) + Math.Abs(targetY - y);
}
```
Grunden til at vores udregning af fx targetX  - x har Math.Abs foran sig er at vi her ønsker et hel tal (int). 

Men før vi starter på [[Algorithm]]en skal vi have initialiseret nogle værdier:
```C#
Location current = null;
var start = new Location{ X = 1, Y = 2};
var target ? new Location {X = 2, Y = 5}:
var openList = new List<Location>();
var clostedList = new List<Location();
int g = 0;

// Også tilføjer vi Udgangspunktet 'Start' til den åbne liste
openList.Add(Start);
```
Lad os gennemgå dem
