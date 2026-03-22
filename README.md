# OdysseyFishCorpseHSK

Мод для **RimWorld 1.6** + **Odyssey** + **Core SK (HSK)**:

- добыча рыбалки Odyssey в виде сырой рыбы (ресурс) заменяется на **трупы рыб** по видам из Core SK (океан / река / болото);
- требуется мод **Harmony** ([brrainz.harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)).

Исходники: [github.com/linyaDev/OdysseyFishCorpseHSK](https://github.com/linyaDev/OdysseyFishCorpseHSK).

## Сборка

Из папки мода (рядом с `RimWorldWin64_Data` и `Mods`):

```bash
dotnet build -c Release
```

В `Assemblies\` появится `OdysseyFishCorpse.dll`. Папку мода положите в `RimWorld\Mods\`.

Ссылки в `OdysseyFishCorpse.csproj` ожидают:

- `..\..\RimWorldWin64_Data\Managed\Assembly-CSharp.dll`
- `..\Core_SK\Assemblies\Core_SK.dll`

Harmony подключается через NuGet только для компиляции; **не** кладите `0Harmony.dll` в `Assemblies` — в игре используется Harmony-мод.

## Установка без сборки

Скопируйте релиз с GitHub (вкладка **Releases**) или соберите локально.
