# Guide to Mapping a Ship for Pax Astralis

## Step-by-Step Instructions

### 1. Initial Setup
1. Open the console in the game.
2. Type `mapping MapId` into the console.
   - `MapId` can be any number, but it is recommended to use a number greater than 100 to avoid conflicts with server-assigned IDs.

### 2. Designing the Ship
1. Design your ship and place down your tiles and objects.
2. **Important:** Avoid using mass spawning of tiles through copy-paste methods as it can cause gridID misplacement. Misplaced gridIDs will result in the ship being placed on different gridIDs, which can lead to server lag and render the ship unusable.
3. Press `F3` to ensure all tiles are on the same gridID.

### 3. Autosaving
- The server will automatically save the ship every 10 minutes. This feature is a safeguard in case anything goes wrong.

### 4. Finalizing the Map
1. When the map design is complete, open the console.
2. Type `fixgridatmos gridID` to make the pressure and air in the ship liveable.
   - To find the `gridID`:
     - Go to the Admin Menu.
     - Navigate to `Objects` -> `Grids`.
     - The ship should typically be the last grid on the list.
     - Right-click the grid and select `View Variables`.
     - The `gridID` will be displayed in the console following the "vv" prefix.

### 5. Naming the Ship
1. In the `View Variables` menu, navigate to `Server Components` and `View Variables` main menu.
2. Change the default name "grid" to your desired ship name.

### 6. Saving the Ship
1. In the console, type `savegrid gridID /shipname.yml`.
   - Do **not** use `savemap` as it will prevent the ship from being sold at the shipyard.

### 7. Testing the Ship
1. To test the ship, run the command `mapinit mapID` in the console.

### 8. Backup and Autosaves
- Always create backups or check the autosaves in case something goes wrong during the process.

### 9. Submitting Your Ship
- Once you are satisfied with your ship, you can submit a pull request to Pax Astralis for review and inclusion in the game.

---

By following these steps, you will ensure that your ship is properly mapped and ready for use in the game. Happy mapping!
