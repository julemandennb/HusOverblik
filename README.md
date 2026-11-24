# 🏠 HusOverblik

**HusOverblik** er et projekt, der har til formål at gøre det nemmere for husstande at administrere, organisere og holde styr på deres varer og ejendele.

Projektet består af en webbaseret løsning og et API, som også skal fungere som backend for en tilhørende mobilapp.

Målet er at give alle medlemmer af en husstand et samlet overblik over:
- Hvilke varer og ejendele husstanden har.
- Hvor de forskellige ting er placeret.
- Hvor meget der er tilbage af de enkelte varer.
- Relevante datoer såsom udløbsdatoer og garantiperioder.

---

## 📋 Funktionalitet

### 1. Husstande og brugere

Brugere skal kunne oprette og administrere husstande.

- En husstand kan have flere tilknyttede brugere.
- En bruger kan være medlem af flere husstande.
- Der anvendes en **many-to-many-relation** mellem brugere og husstande.
- Relationstabellen indeholder en `role`, som definerer brugerens rettigheder i den enkelte husstand.
- Medlemmer af samme husstand deler adgang til husstandens varer og zoner.

### 2. Zoner

Hver husstand kan oprette sine egne zoner til organisering og opbevaring af varer og ejendele.

En zone skal indeholde en temperaturtype (`temperature_type`), som beskriver de forhold, zonen er beregnet til.

| Temperaturtype | Beskrivelse | Eksempel |
|---|---|---|
| `Cold` | Kold opbevaring | Køleskab |
| `Frozen` | Frost | Fryser |
| `Hot` | Varm opbevaring | Opvarmet opbevaringsområde |
| `Room Temperature` | Stuetemperatur | Køkkenskab, depotrum |
| `Custom` | Brugerdefineret | Andre opbevaringsforhold |

Eksempler på zoner:

- Køleskab
- Fryser
- Køkkenskab
- Badeværelse
- Garage
- Værksted
- Kælderrum

### 3. Varer (Items)

Hver vare skal som minimum indeholde følgende oplysninger:

| Felt | Beskrivelse |
|---|---|
| `name` | Varens navn |
| `quantity` | Den samlede mængde |
| `measurement` | Måleenhed, f.eks. gram, kilogram, liter eller milliliter |
| `pieces` | Antal pakker eller enheder |
| `date` | En relevant dato, f.eks. indkøbsdato, udløbsdato eller garantidato |
| `zone_id` | Reference til den zone, hvor varen er placeret |
| `tags` | Et eller flere tags til kategorisering |

Brugerne skal kunne:
- Oprette varer.
- Redigere eksisterende varer.
- Slette varer.
- Flytte varer mellem zoner.
- Tilknytte flere tags til samme vare.

### 4. Tags og kategorisering

Tags bruges til at kategorisere og filtrere husstandens varer.

Funktionaliteten skal omfatte:

- Seed med standardtags.
- Mulighed for selv at oprette tags.
- Mulighed for at redigere og slette egne tags.
- Mulighed for at tilknytte flere tags til en vare.
- Filtrering af varer baseret på tags.

#### Mad-relaterede tags

Hvert tag skal have en boolean-egenskab kaldet `is_food`, som angiver, om tagget repræsenterer en madvare.

Egenskaben skal kunne indstilles via en checkbox, når et tag oprettes eller redigeres.

Eksempler på standardtags:

| Tag | `is_food` |
|---|---|
| Fødevarer | `true` |
| Kød | `true` |
| Grøntsager | `true` |
| Mejeriprodukter | `true` |
| Frugt | `true` |
| Rengøring | `false` |
| Elektronik | `false` |
| Værktøj | `false` |
| Personlig pleje | `false` |
| Reservedele | `false` |

Tags med `is_food = true` skal senere bruges af opskriftsfunktionen til at identificere relevante madvarer.

---

## 🔔 Fremtidig funktionalitet

### 5. Notifikationer og automatisering

Der skal på sigt udvikles en jobfunktion, som automatisk håndterer påmindelser og notifikationer.

Planlagte funktioner:

- E-mailnotifikationer til brugerne.
- Push-notifikationer til mobilappen.
- Påmindelser om varer, der nærmer sig udløbsdatoen.
- Påmindelser om garantiperioder og andre relevante datoer.
- Automatiske jobs, der kører på bestemte tidspunkter.

### 6. Opskrifter

En fremtidig funktion skal gøre det muligt at finde opskrifter baseret på de madvarer, husstanden allerede har registreret.

Funktionen skal integreres med et eksternt opskrifts-API.

Planlagte funktioner:

- Hente opskrifter fra et eksternt API.
- Identificere madvarer via tags med `is_food = true`.
- Sammenligne opskrifternes ingredienser med husstandens varer.
- Vise opskrifter baseret på tilgængelige ingredienser.
- Vise hvilke ingredienser der mangler til en opskrift.

**Formål:** At gøre det nemmere at udnytte de varer, man allerede har, og dermed reducere madspild.

> Bemærk: Opskrifts-API'et og opskriftsfunktionen er ikke en del af den første udviklingsfase. De implementeres på et senere tidspunkt.

---

## 🏗️ Teknisk arkitektur

Projektet består af følgende hovedkomponenter:

| Komponent | Beskrivelse |
|---|---|
| **Website** | Webbaseret brugergrænseflade til administration af husstande, zoner, varer og tags. |
| **API** | Backend, der håndterer data, forretningslogik og kommunikation med web og mobilapp. |
| **Database** | Opbevaring af brugere, husstande, relationer, zoner, varer og tags. |
| **Jobfunktion** | Baggrundsopgaver til automatisering og notifikationer. |
| **Eksterne integrationer** | Fremtidige integrationer, herunder opskrifts-API. |

API'et skal fungere som et fælles backend-lag for både hjemmesiden og mobilappen.

Arkitekturen skal være modulær og fleksibel, så det er muligt at udvide systemet med nye funktioner og integrationer senere.

---

## 🗺️ Roadmap

### Første udviklingsfase (MVP)

- [ ] Oprettelse og administration af brugere.
- [ ] Oprettelse og administration af husstande.
- [ ] Many-to-many-relation mellem brugere og husstande.
- [ ] Rollebaserede rettigheder i husstande.
- [ ] Oprettelse og administration af zoner.
- [ ] Understøttelse af temperaturtyper.
- [ ] CRUD-funktionalitet for varer.
- [ ] Oprettelse og administration af tags.
- [ ] Seed af standardtags.
- [ ] Understøttelse af `is_food`.
- [ ] Filtrering af varer via tags.

### Senere udviklingsfaser

- [ ] Automatisk jobfunktion.
- [ ] E-mailnotifikationer.
- [ ] Push-notifikationer.
- [ ] Påmindelser om udløbsdatoer.
- [ ] Påmindelser om garantiperioder.
- [ ] Integration med eksternt opskrifts-API.
- [ ] Opskrifter baseret på husstandens eksisterende varer.
- [ ] Oversigt over manglende ingredienser.

---

## 🎯 Projektets formål

HusOverblik skal være et samlet system til administration af en husstands varer og ejendele.

Ved at kombinere organisering, kategorisering, datooverblik og fremtidige automatiseringer skal løsningen gøre hverdagen nemmere, give et bedre overblik og hjælpe med at reducere madspild.

**HusOverblik – ét samlet overblik over hjemmets varer og ejendele.**