# AutoRestart Mod for Megabonk

This Melonloader mod automates the process of restarting the game to find specific items in the shops (the "Shady Guys"). It's designed to help you quickly start a run with your desired items without manual restarts.

## How It Works

On the first stage, the mod will:
1. Check the inventories of all "Shady Guy" NPCs.
2. If the required items (as defined in your config) are present across the shops, the mod will do nothing, and you can start your run.
3. If the required items are not found, the mod will automatically restart the run. This cycle continues until a valid seed is found.

## Configuration

You can configure the mod by editing the `MelonLoader/UserData/AutoRestart.cfg` file.

- **`RequiredItemNames`**: A comma-separated list of the item names you want to find. The mod will restart until all listed items are available for purchase from the Shady Guys. The items both most be purchaseable, so found in different Shady Guy inventories. Requesting too many items, or even too many rare items, will result in endless resets. The game is still bound by time.
  
  *Example:*
  ```
  RequiredItemNames = SoulHarvester,CreditCardGreen
  ```

- **`DebugLogging`**: Set to `true` to enable detailed logging in the MelonLoader console for troubleshooting. Defaults to `false`.

## Items

*   `Key`
*   `Beer`
*   `SpikyShield`
*   `Bonker`
*   `SlipperyRing`
*   `CowardsCloak`
*   `GymSauce`
*   `Battery`
*   `PhantomShroud`
*   `ForbiddenJuice`
*   `DemonBlade`
*   `GrandmasSecretTonic`
*   `GiantFork`
*   `MoldyCheese`
*   `GoldenSneakers`
*   `SpicyMeatball`
*   `Chonkplate`
*   `LightningOrb`
*   `IceCube`
*   `DemonicBlood`
*   `DemonicSoul`
*   `BeefyRing`
*   `Dragonfire`
*   `GoldenGlove`
*   `GoldenShield`
*   `ZaWarudo`
*   `OverpoweredLamp`
*   `Feathers`
*   `Ghost`
*   `SluttyCannon`
*   `TurboSocks`
*   `ShatteredWisdom`
*   `EchoShard`
*   `SuckyMagnet`
*   `Backpack`
*   `Clover`
*   `Campfire`
*   `Rollerblades`
*   `Skuleg`
*   `EagleClaw`
*   `Scarf`
*   `Anvil`
*   `Oats`
*   `CursedDoll`
*   `EnergyCore`
*   `ElectricPlug`
*   `BobDead`
*   `SoulHarvester`
*   `Mirror`
*   `JoesDagger`
*   `WeebHeadset`
*   `SpeedBoi`
*   `Gasmask`
*   `ToxicBarrel`
*   `HolyBook`
*   `BrassKnuckles`
*   `IdleJuice`
*   `Kevin`
*   `Borgar`
*   `Medkit`
*   `GamerGoggles`
*   `UnstableTransfusion`
*   `BloodyCleaver`
*   `CreditCardRed`
*   `CreditCardGreen`
*   `BossBuster`
*   `LeechingCrystal`
*   `TacticalGlasses`
*   `Cactus`
*   `CageKey`
*   `IceCrystal`
*   `TimeBracelet`
*   `GloveLightning`
*   `GlovePoison`
*   `GloveBlood`
*   `GloveCurse`
*   `GlovePower`
*   `Wrench`
*   `Beacon`
*   `GoldenRing`
*   `QuinsMask`

## Disclaimer

**This mod can be considered a cheat.**

It removes the random nature of runs and allows you to force an ideal set of starting items.
