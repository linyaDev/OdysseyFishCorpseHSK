# OdysseyFishCorpseHSK

Source: [github.com/linyaDev/OdysseyFishCorpseHSK](https://github.com/linyaDev/OdysseyFishCorpseHSK).

## English

Mod for **RimWorld 1.6** + **Odyssey** + **Core SK (HSK)**:

- Odyssey fishing yields raw fish items; this mod **replaces those with fish corpses** (Core SK species: ocean / river / marsh).
- Requires **Harmony** ([brrainz.harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)).

## Русский

Мод для **RimWorld 1.6** + **Odyssey** + **Core SK (HSK)**:

- добыча рыбалки Odyssey в виде сырой рыбы (ресурс) заменяется на **трупы рыб** по видам из Core SK (океан / река / болото);
- требуется мод **Harmony** ([brrainz.harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)).

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

## Build (English)

From the mod folder (next to `RimWorldWin64_Data` and `Mods`):

```bash
dotnet build -c Release
```

Output: `Assemblies\OdysseyFishCorpse.dll`. Copy the whole mod folder into `RimWorld\Mods\`.

`OdysseyFishCorpse.csproj` expects:

- `..\..\RimWorldWin64_Data\Managed\Assembly-CSharp.dll`
- `..\Core_SK\Assemblies\Core_SK.dll`

Harmony is referenced via NuGet for compile only — **do not** ship `0Harmony.dll` in `Assemblies`; use the Harmony mod in-game.

## Install without building

Use a **Releases** build from GitHub or compile locally.
