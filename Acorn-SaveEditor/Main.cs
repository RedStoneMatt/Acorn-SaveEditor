using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Force.Crc32;

namespace Acorn_SaveEditor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        SaveData thisSave = new SaveData();
        string originalPath = "";
        List<byte> rawdata = new List<byte>();
        char[] rawstring;
        int fileIndex = 0;
        
        Image[] itemImages = { Properties.Resources.Mushroom, Properties.Resources.Fire, Properties.Resources.Star, Properties.Resources.Ice, null, Properties.Resources.Propeller, Properties.Resources.Mini, Properties.Resources.Penguin, Properties.Resources.Acorn, Properties.Resources.P_Acorn, null, null, null, null, Properties.Resources.Empty };


        private void Main_Load(object sender, EventArgs e)
        {
            saveFileToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                string output;
                dialog.Filter = "NSMBU Save File (*.sav; *.dat)|*.sav;*.dat|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    thisSave = new SaveData();
                    output = dialog.FileName;
                    thisSave.loadFile(output, ref rawdata);
                    originalPath = output;
                    this.Text = "Acorn Save Editor - " + output.Split('\\')[output.Split('\\').Length - 1];

                    profileTabControl.Enabled = true;
                    saveFileToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;

                    loadHeader();
                    loadPlayerProperties();
                    loadMiscellaneous();
                    loadSecretIsland();
                    loadInventory();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveHeader();
            savePlayerProperties();
            saveMiscellaneous();
            saveSecretIsland();

            List<byte> outputFile = new List<byte>();
            outputFile = thisSave.saveFile();
            using (FileStream outputBin = new FileStream(originalPath, FileMode.Create))
            {
                for (int i = 0; i < outputFile.ToArray().Length; i++)
                {
                    outputBin.WriteByte(outputFile[i]);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveHeader();
            savePlayerProperties();
            saveMiscellaneous();
            saveSecretIsland();

            SaveFileDialog textDialog;
            textDialog = new SaveFileDialog();
            textDialog.Filter = "NSMBU Save File (*.sav; *.dat)|*.sav;*.dat|All files (*.*)|*.*";
            if (textDialog.ShowDialog() == DialogResult.OK)
            {
                //Stream things to get the saved path
                System.IO.Stream fileStream = textDialog.OpenFile();
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream);
                string outputPath = ((FileStream)(sw.BaseStream)).Name;
                sw.Close();
                originalPath = outputPath;
                this.Text = "Acorn Save Editor - " + outputPath.Split('\\')[outputPath.Split('\\').Length - 1];

                //Save it
                List<byte> outputFile = new List<byte>();
                outputFile = thisSave.saveFile();
                using (FileStream outputBin = new FileStream(outputPath, FileMode.Create))
                {
                    for (int i = 0; i < outputFile.ToArray().Length; i++)
                    {
                        outputBin.WriteByte(outputFile[i]);
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Acorn Save Editor is a program which can edit and save New Super Mario Bros. U Save Files.\r\n\r\n"
                          + "Programmed by RedStoneMatt\r\n"
                          + "Save File Documentation by Kinnay\r\n"
                          + "Crc32.NET library by force\r\n"
                          + "Crc16 function wrote by Marc Gravell\r\n\r\nv0.1", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void loadHeader()
        {
            PictureBox[] files = { file1PictureBox, file2PictureBox, file3PictureBox };
            foreach (PictureBox thisFile in files)
            {
                if(thisSave.header.fileIndex == Convert.ToInt32(thisFile.Name.Substring(4, 1))-1)
                {
                    thisFile.Image = Properties.Resources.FileSelected;
                }
                else
                {
                    thisFile.Image = null;
                }
            }
            hasBeenNotifiedRDashCheckBox.Checked = Convert.ToBoolean(thisSave.header.hasBeenNotifiedRDash);
            buttonLayoutPictureBox.Image = ((thisSave.header.buttonLayout == 0) ? Properties.Resources.Controls1 : Properties.Resources.Controls2);
        }

        public void loadPlayerProperties()
        {
            NumericUpDown[] playerLives = { player0LivesNumUpDown, player1LivesNumUpDown, player2LivesNumUpDown, player3LivesNumUpDown };
            NumericUpDown[] playerContinues = { player0ContinuesNumUpDown, player1ContinuesNumUpDown, player2ContinuesNumUpDown, player3ContinuesNumUpDown };
            ComboBox[] playerPowerups = { player0PowerupComboBox, player1PowerupComboBox, player2PowerupComboBox, player3PowerupComboBox };
            GroupBox[] playerGroupBoxes = { player0GroupBox, player1GroupBox, player2GroupBox, player3GroupBox };
            for (int i = 0; i < 4; i++)
            {
                string[] playerNames = { "Mario", "Luigi", "BToad", "Ytoad" };
                playerLives[i].Value = thisSave.profiles[fileIndex].lives[i];
                playerContinues[i].Value = thisSave.profiles[fileIndex].continues[i];
                playerPowerups[i].SelectedIndex = thisSave.profiles[fileIndex].powerups[i];
                playerGroupBoxes[i].Text = "Player " + thisSave.profiles[fileIndex].characters[i] + " - " + playerNames[thisSave.profiles[0].characters[i]];
            }
        }

        public void loadMiscellaneous()
        {
            isValidCheckBox.Checked = ((fileIndex < 6) ? !Convert.ToBoolean(thisSave.profiles[fileIndex].isValid) : Convert.ToBoolean(thisSave.profiles[fileIndex].isValid));
            lastNumPlayersNumUpDown.Value = thisSave.profiles[fileIndex].lastNumPlayers;
            coinsNumUpDown.Value = thisSave.profiles[fileIndex].coins;
            rockWorldSwitchStateButton.Image = ((Convert.ToBoolean(thisSave.profiles[fileIndex].rockWorldSwitchState)) ? Properties.Resources.BlueSwitch : Properties.Resources.RedSwitch);

            scoreNumUpDown.Value = thisSave.profiles[fileIndex].score;
            bestCreditsCoinsNumUpDown.Value = thisSave.profiles[fileIndex].bestCreditsCoins;
            nabbitWorldNumUpDown.Value = thisSave.profiles[fileIndex].nabbitWorld;
            nabbitWorld2NumUpDown.Value = thisSave.profiles[fileIndex].nabbitWorld2;
            activeBabyYoshiComboBox.SelectedIndex = thisSave.profiles[fileIndex].activeBabyYoshi;
            levelCounterForBalloonYoshiNumUpDown.Value = thisSave.profiles[fileIndex].levelCounterForBalloonYoshi;
            levelCounterForBubbleYoshiNumUpDown.Value = thisSave.profiles[fileIndex].levelCounterForBubbleYoshi;
            caughtNabbitFlagsNumUpDown.Value = thisSave.profiles[fileIndex].caughtNabbitFlags;
        }

        public void loadSecretIsland()
        {
            totalCoinsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.totalCoins;
            totalStarCoinsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.totalStarCoins;
            goalsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.goals;
            clappingsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.clappings;
            boostBlockDistanceNumUpDown.Value = (decimal)thisSave.profiles[fileIndex].totalStats.boostBlockDistance / 100;
            miniBoostBlockDistanceNumUpDown.Value = (decimal)thisSave.profiles[fileIndex].totalStats.miniBoostBlockDistance / 100;
            caughtNabbitsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.caughtNabbits;
            stompedGoombasNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.stompedGoombas;
            collectedItemsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.collectedItems;
            goalsWithYoshiNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.goalsWithYoshi;
            goalsWithBabyYoshiNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.goalsWithBabyYoshi;
            goalOneUpsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.goalOneUps;
            goalFireworksNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.goalFireworks;
            threeUpMoonsNumUpDown.Value = thisSave.profiles[fileIndex].totalStats.threeUpMoons;
        }

        public void loadInventory()
        {
            foreach (PictureBox thisBSlotPic in inventoryBackgroundPictureBox.Controls.OfType<PictureBox>())
            {
                foreach (PictureBox thisItemPic in thisBSlotPic.Controls.OfType<PictureBox>())
                {
                    int itemID = Convert.ToInt32(thisItemPic.Name.Substring(13, 1));
                    thisItemPic.Image = itemImages[thisSave.profiles[fileIndex].inventoryItems[itemID]];
                }
            }
        }

        public void saveHeader()
        {
            thisSave.header.hasBeenNotifiedRDash = Convert.ToByte(hasBeenNotifiedRDashCheckBox.Checked);
        }

        public void savePlayerProperties()
        {
            NumericUpDown[] playerLives = { player0LivesNumUpDown, player1LivesNumUpDown, player2LivesNumUpDown, player3LivesNumUpDown };
            NumericUpDown[] playerContinues = { player0ContinuesNumUpDown, player1ContinuesNumUpDown, player2ContinuesNumUpDown, player3ContinuesNumUpDown };
            ComboBox[] playerPowerups = { player0PowerupComboBox, player1PowerupComboBox, player2PowerupComboBox, player3PowerupComboBox };
            for (int i = 0; i < 4; i++)
            {
                thisSave.profiles[fileIndex].lives[i] = (byte)playerLives[i].Value;
                thisSave.profiles[fileIndex].continues[i] = (byte)playerContinues[i].Value;
                thisSave.profiles[fileIndex].powerups[i] = (byte)playerPowerups[i].SelectedIndex;
            }
        }

        public void saveMiscellaneous()
        {
            thisSave.profiles[fileIndex].isValid = Convert.ToByte(((fileIndex < 6) ? !isValidCheckBox.Checked : isValidCheckBox.Checked));
            thisSave.profiles[fileIndex].lastNumPlayers = (byte)lastNumPlayersNumUpDown.Value;
            thisSave.profiles[fileIndex].coins = (byte)coinsNumUpDown.Value;
            thisSave.profiles[fileIndex].score = (uint)scoreNumUpDown.Value;
            thisSave.profiles[fileIndex].bestCreditsCoins = (ushort)bestCreditsCoinsNumUpDown.Value;
            thisSave.profiles[fileIndex].nabbitWorld = (byte)nabbitWorldNumUpDown.Value;
            thisSave.profiles[fileIndex].nabbitWorld2 = (byte)nabbitWorld2NumUpDown.Value;
            thisSave.profiles[fileIndex].activeBabyYoshi = (byte)activeBabyYoshiComboBox.SelectedIndex;
            thisSave.profiles[fileIndex].levelCounterForBalloonYoshi = (byte)levelCounterForBalloonYoshiNumUpDown.Value;
            thisSave.profiles[fileIndex].levelCounterForBubbleYoshi = (byte)levelCounterForBubbleYoshiNumUpDown.Value;
            thisSave.profiles[fileIndex].caughtNabbitFlags = (byte)caughtNabbitFlagsNumUpDown.Value;
        }

        public void saveSecretIsland()
        {
            thisSave.profiles[fileIndex].totalStats.totalCoins = (uint)totalCoinsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.totalStarCoins = (uint)totalStarCoinsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.goals = (uint)goalsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.clappings = (uint)clappingsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.boostBlockDistance = (uint)(boostBlockDistanceNumUpDown.Value * 100);
            thisSave.profiles[fileIndex].totalStats.miniBoostBlockDistance = (uint)(miniBoostBlockDistanceNumUpDown.Value * 100);
            thisSave.profiles[fileIndex].totalStats.caughtNabbits = (uint)caughtNabbitsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.stompedGoombas = (uint)stompedGoombasNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.collectedItems = (uint)collectedItemsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.goalsWithYoshi = (uint)goalsWithYoshiNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.goalsWithBabyYoshi = (uint)goalsWithBabyYoshiNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.goalOneUps = (uint)goalOneUpsNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.goalFireworks = (uint)goalFireworksNumUpDown.Value;
            thisSave.profiles[fileIndex].totalStats.threeUpMoons = (uint)threeUpMoonsNumUpDown.Value;
        }

        private void save1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveHeader();
            savePlayerProperties();
            saveMiscellaneous();
            saveSecretIsland();

            ToolStripMenuItem thisToolStripMenuItem = (sender as ToolStripMenuItem);
            int isNSLU = (thisToolStripMenuItem.Name.EndsWith("1") ? 3 : 0);
            int isQuick = (thisToolStripMenuItem.Name.Contains("quick") ? 6 : 0);
            int fileNum = Convert.ToInt32(thisToolStripMenuItem.Name.Substring(((isQuick > 0) ? 9 : 4), 1));
            fileIndex = fileNum - 1 + isQuick + isNSLU;
            if(isQuick > 0) { isValidCheckBox.Text = ":Is Quick Save Active"; isValidCheckBox.Location = new Point(75, isValidCheckBox.Location.Y); }
            else            { isValidCheckBox.Text = ":New Save File"; isValidCheckBox.Location = new Point(106, isValidCheckBox.Location.Y); }

            loadHeader();
            loadPlayerProperties();
            loadMiscellaneous();
            loadSecretIsland();
            loadInventory();
        }


        private void inventoryItemPictureBox_Click(object sender, EventArgs e)
        {
            byte[] nextItem = { 1, 2, 3, 5, 0, 6, 7, 8, 9, 0xE, 0, 0, 0, 0, 0 };

            PictureBox thisItem = (sender as PictureBox);
            int itemID = Convert.ToInt32(thisItem.Name.Substring(13, 1));
            int currentItem = thisSave.profiles[fileIndex].inventoryItems[itemID];

            thisSave.profiles[fileIndex].inventoryItems[itemID] = nextItem[currentItem];

            thisItem.Image = itemImages[nextItem[currentItem]];
        }

        private void buttonLayoutPictureBox_Click(object sender, EventArgs e)
        {
            Image[] buttonLayouts = { Properties.Resources.Controls1, Properties.Resources.Controls2 };
            thisSave.header.buttonLayout = Convert.ToByte(!(Convert.ToBoolean(thisSave.header.buttonLayout)));
            buttonLayoutPictureBox.Image = buttonLayouts[thisSave.header.buttonLayout];
        }

        public void editLocation(ref SaveCSLocation locationToEdit)
        {
            EditLocation editLocation = new EditLocation(locationToEdit);
            DialogResult dialogresult = editLocation.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                locationToEdit = editLocation.thisSaveCSLocation;
                editLocation.Close();
            }
        }

        private void editLocationButton_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Name.Contains("playerLoc1")) { editLocation(ref thisSave.profiles[fileIndex].playerLoc1); }
            if ((sender as Button).Name.Contains("playerLoc2")) { editLocation(ref thisSave.profiles[fileIndex].playerLoc2); }
            if ((sender as Button).Name.Contains("airshipLocation")) { editLocation(ref thisSave.profiles[fileIndex].airshipLocation); }
        }

        private void filePictureBox_Click(object sender, EventArgs e)
        {
            PictureBox thisPictureBox = (sender as PictureBox);
            int fileID = Convert.ToInt32(thisPictureBox.Name.Substring(4, 1)) - 1;
            PictureBox[] files = { file1PictureBox, file2PictureBox, file3PictureBox };
            foreach(PictureBox thisFile in files)
            {
                thisFile.Image = null;
            }
            thisPictureBox.Image = Properties.Resources.FileSelected;
            thisSave.header.fileIndex = (byte)fileID;
        }

        private void editFlagsButton_Click(object sender, EventArgs e)
        {
            string name = (sender as Button).Name.Substring(0, (sender as Button).Name.Length - 15);
            Console.WriteLine(name);
            if (name == "worldsBeatenFlags") {
                string[] texts =
                {
                    "Has Game Started",
                    "World 1 Beaten",
                    "World 2 Beaten",
                    "World 3 Beaten",
                    "World 4 Beaten",
                    "World 5 Beaten",
                    "World 6 Beaten",
                    "World 7 Beaten",
                    "World 8 and Game Beaten"
                };
                editFlags(ref thisSave.profiles[fileIndex].worldsBeatenFlags, texts);
            }
            if (name == "gameCompletionFlags") {
                string[] texts =
                {
                    "???",
                    "All Levels Beaten",
                    "???",
                    "All Star Coins Collected",
                    "All Exists Reached"
                };
                editFlags(ref thisSave.profiles[fileIndex].gameCompletionFlags, texts);
            }
            if (name == "availableBabyYoshiFlags") {
                string[] texts =
                {
                    "Balloon Yoshi (Acorn Plains)",
                    "Balloon Yoshi (Sparkling Waters)",
                    "Bubble Yoshi (Frosted Glacier)",
                    "Bubble Yoshi (Rock-Candy Mines)"
                };
                editFlags(ref thisSave.profiles[fileIndex].availableBabyYoshiFlags, texts);
            }
        }

        public void editFlags(ref ushort flag, string[] flagTexts)
        {
            EditFlags editFlag = new EditFlags(flag, flagTexts);
            DialogResult dialogresult = editFlag.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                flag = editFlag.thisFlag;
                editFlag.Close();
            }
        }

        public void editFlags(ref byte flag, string[] flagTexts)
        {
            EditFlags editFlag = new EditFlags(flag, flagTexts);
            DialogResult dialogresult = editFlag.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                flag = (byte)editFlag.thisFlag;
                editFlag.Close();
            }
        }

        private void rockWorldSwitchStateButton_Click(object sender, EventArgs e)
        {
            thisSave.profiles[fileIndex].rockWorldSwitchState = Convert.ToByte(!Convert.ToBoolean(thisSave.profiles[fileIndex].rockWorldSwitchState));
            rockWorldSwitchStateButton.Image = ((Convert.ToBoolean(thisSave.profiles[fileIndex].rockWorldSwitchState)) ? Properties.Resources.BlueSwitch : Properties.Resources.RedSwitch);
        }
    }
}
