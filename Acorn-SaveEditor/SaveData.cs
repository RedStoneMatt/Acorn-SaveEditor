using Force.Crc32; //I'm actually wondering what's the point of checksums. Cuz like, it's supposed to prevent fucked-up saves to be loaded, but nothing prevents you to fuck up the sazve and recalculate the checksum
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acorn_SaveEditor
{

    public class SaveData                                   //Save File documentation by Kinnay (and maybe others) - Thanks man !
    {
        public List<byte> saveFile()
        {
            List<byte> thisData = new List<byte>();

            /********/
            /*header*/
            /********/
            addNextInt32(ref thisData, this.header.header);
            thisData.Add(this.header.always1);
            thisData.Add(this.header.always0);
            thisData.Add(this.header.always2);
            thisData.Add(this.header.fileIndex);
            thisData.Add(this.header.hasBeenNotifiedRDash);
            thisData.Add(this.header.buttonLayout);
            thisData.Add(this.header.field_A);
            thisData.Add(this.header.field_B);
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, 0, 12)));
            
            /*************************/
            /*profiles & tempProfiles*/
            /*************************/
            foreach(ProfileSaveData thisProfile in this.profiles)
            {
                int startIndex = thisData.Count;

                thisData.Add(thisProfile.isValid);
                thisData.Add(thisProfile.lastNumPlayers);
                thisData.Add(thisProfile.coins);
                thisData.Add(thisProfile.rockWorldSwitchState);
                for(int i = 0; i < 2; i++)
                {
                    SaveCSLocation thisSaveCSLocation = ((i == 0) ? thisProfile.playerLoc1 : thisProfile.playerLoc2);
                    thisData.Add(thisSaveCSLocation.world);
                    thisData.Add(thisSaveCSLocation.section);
                    thisData.Add(thisSaveCSLocation.node);
                }
                thisData.Add(thisProfile.field_A);
                thisData.Add(thisProfile.field_B);
                for(int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.lives[i]);
                }
                for(int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.characters[i]);
                }
                for(int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.powerups[i]);
                }
                for(int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.continues[i]);
                }
                addNextInt16(ref thisData, thisProfile.worldsBeatenFlags);
                thisData.Add(thisProfile.field_1E);
                thisData.Add(thisProfile.field_1F);
                addNextFloat(ref thisData, thisProfile.vec3.x);
                addNextFloat(ref thisData, thisProfile.vec3.y);
                addNextFloat(ref thisData, thisProfile.vec3.z);
                thisData.Add(thisProfile.field_2C);
                thisData.Add(thisProfile.field_2D);
                thisData.Add(thisProfile.gameCompletionFlags);
                thisData.Add(thisProfile.field_2F);
                addNextInt32(ref thisData, thisProfile.score);
                for (int i = 0; i < 16; i++)
                {
                    thisData.Add(thisProfile.field_34[i]);
                }
                addNextInt16(ref thisData, thisProfile.bestCreditsCoins);
                thisData.Add(thisProfile.airshipLocation.world);
                thisData.Add(thisProfile.airshipLocation.section);
                thisData.Add(thisProfile.airshipLocation.node);
                thisData.Add(thisProfile.nabbitWorld);
                thisData.Add(thisProfile.nabbitWorld2);
                thisData.Add(thisProfile.field_4B);
                thisData.Add(thisProfile.field_4C);
                thisData.Add(thisProfile.field_4D);
                thisData.Add(thisProfile.babyYoshiItem);
                thisData.Add(thisProfile.field_4F);
                thisData.Add(thisProfile.field_50);
                thisData.Add(thisProfile.field_51);
                thisData.Add(thisProfile.field_52);
                thisData.Add(thisProfile.field_53);
                thisData.Add(thisProfile.field_54);
                thisData.Add(thisProfile.availableBabyYoshiFlags);
                thisData.Add(thisProfile.activeBabyYoshi);
                thisData.Add(thisProfile.field_57);
                thisData.Add(thisProfile.field_58);
                thisData.Add(thisProfile.field_59);
                thisData.Add(thisProfile.levelCounterForBalloonYoshi);
                thisData.Add(thisProfile.levelCounterForBubbleYoshi);
                for (int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.ambushItemCounters[i]);
                }
                for (int i = 0; i < 4; i++)
                {
                    thisData.Add(thisProfile.ambushItems[i]);
                }
                for (int i = 0; i < 7; i++)
                {
                    thisData.Add(thisProfile.remainingNabbitAttempts[i]);
                }
                for (int i = 0; i < 123; i++)
                {
                    thisData.Add(thisProfile.levelFlags[i]);
                }
                for (int i = 0; i < 62; i++)
                {
                    thisData.Add(thisProfile.levelDeathCounters[i]);
                }
                for (int i = 0; i < 41; i++)
                {
                    thisData.Add(thisProfile.starCoinCollection[i]);
                }
                for (int i = 0; i < 10; i++)
                {
                    thisData.Add(thisProfile.inventoryItems[i]);
                }

                List<AmbushSaveEnemy> thisAmbushSaveEnemyList = thisProfile.ambushState.world1.Concat(thisProfile.ambushState.world2).Concat(thisProfile.ambushState.world3).Concat(thisProfile.ambushState.world4).Concat(thisProfile.ambushState.world5).Concat(thisProfile.ambushState.world7).ToList();
                foreach (AmbushSaveEnemy thisAmbushSaveEnemy in thisAmbushSaveEnemyList) {
                    thisData.Add(thisAmbushSaveEnemy.location.world);
                    thisData.Add(thisAmbushSaveEnemy.location.section);
                    thisData.Add(thisAmbushSaveEnemy.location.node);
                    thisData.Add(thisAmbushSaveEnemy.field_3);
                    thisData.Add(thisAmbushSaveEnemy.field_4);
                }
                for (int i = 0; i < 18; i++)
                {
                    thisData.Add(thisProfile.ambushState.field_5A[i]);
                }
                thisData.Add(thisProfile.ambushState.field_6C);

                addNextInt32(ref thisData, thisProfile.totalStats.totalCoins);
                addNextInt32(ref thisData, thisProfile.totalStats.totalStarCoins);
                addNextInt32(ref thisData, thisProfile.totalStats.goals);
                addNextInt32(ref thisData, thisProfile.totalStats.clappings);
                addNextInt32(ref thisData, thisProfile.totalStats.boostBlockDistance);
                addNextInt32(ref thisData, thisProfile.totalStats.miniBoostBlockDistance);
                addNextInt32(ref thisData, thisProfile.totalStats.caughtNabbits);
                addNextInt32(ref thisData, thisProfile.totalStats.stompedGoombas);
                addNextInt32(ref thisData, thisProfile.totalStats.collectedItems);
                addNextInt32(ref thisData, thisProfile.totalStats.goalsWithYoshi);
                addNextInt32(ref thisData, thisProfile.totalStats.goalsWithBabyYoshi);
                addNextInt32(ref thisData, thisProfile.totalStats.goalOneUps);
                addNextInt32(ref thisData, thisProfile.totalStats.goalFireworks );
                addNextInt32(ref thisData, thisProfile.totalStats.threeUpMoons);
                thisData.Add(thisProfile.caughtNabbitFlags);
                for (int i = 0; i < 3; i++)
                {
                    thisData.Add(thisProfile.padding[i]);
                }
                addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, startIndex, thisData.Count)));
            }
            
            /***************/
            /*challengeData*/
            /***************/
            int challengeStartIndex = thisData.Count;
            thisData.Add(this.challengeData.initialized);
            for (int i = 0; i < 5; i++)
            {
                thisData.Add(this.challengeData.categoryMedals[i]);
            }
            thisData.Add(this.challengeData.field_6);
            thisData.Add(this.challengeData.field_7);
            foreach(ChallengeSave thisChallengeSave in this.challengeData.challenges)
            {
                int replayStartIndex = thisData.Count;

                thisData.Add(thisChallengeSave.state);
                thisData.Add(thisChallengeSave.player);
                thisData.Add(thisChallengeSave.field_2);
                thisData.Add(thisChallengeSave.field_3);

                int fflStartIndex = thisData.Count;
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_0);
                int authorIDStartIndex = thisData.Count;
                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_0);
                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_2);
                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_4);
                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_6);
                ushort authorIdCRC16 = CRC16.ComputeChecksum(sliceByteList(thisData, authorIDStartIndex, thisData.Count));
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.flags);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.field_1);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.field_2);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.index);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_0);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_1);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_2);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_3);
                addNextInt16(ref thisData, authorIdCRC16);

                for (int i = 0; i < 2; i++)
                {
                    thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_16[i]);
                }

                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_18);

                for(int i = 0; i < 10; i++)
                {
                    addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.miiName[i]);
                }

                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_2E);
                thisData.Add(thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_2F);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_30);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_34);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_38);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_3C);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_40);
                addNextInt32(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_44);

                for (int i = 0; i < 10; i++)
                {
                    addNextInt16(ref thisData, thisChallengeSave.fflStoreData.miiDataOfficial.creatorName[i]);
                }

                addNextInt16(ref thisData, thisChallengeSave.fflStoreData.field_5C);
                addNextInt16(ref thisData, CRC16.ComputeChecksum(sliceByteList(thisData, fflStartIndex, thisData.Count)));
                addNextInt32(ref thisData, thisChallengeSave.timer);
                addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, replayStartIndex, thisData.Count)));
            }
            for(int i = 0; i < 5184; i++)
            {
                thisData.Add(this.challengeData.field_21C8[i]);
            }
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, challengeStartIndex, thisData.Count)));
            
            /*******************/
            /*BoostRushSaveData*/
            /*******************/
            int boostStartIndex = thisData.Count;
            thisData.Add(this.boostRushData.field_0);
            thisData.Add(this.boostRushData.field_1);
            thisData.Add(this.boostRushData.field_2);
            thisData.Add(this.boostRushData.field_3);
            for(int i = 0; i < 32; i++)
            {
                addNextInt32(ref thisData, this.boostRushData.secondHundreds[i]);
            }
            for (int i = 0; i < 32; i++)
            {
                thisData.Add(this.boostRushData.field_84[i]);
            }
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, boostStartIndex, thisData.Count)));
            
            /**************/
            /*coinEditData*/
            /**************/
            int coinEditDataSelectedIndex = thisData.Count;
            foreach (CoinEditPatternStage thisCourse in this.coinEditData.courses)
            {
                foreach(CoinEditCoin thisCoin in thisCourse.coins)
                {
                    thisData.Add(thisCoin.area);
                    thisData.Add(thisCoin.zone);
                    thisData.Add(thisCoin.isValid);
                    thisData.Add(thisCoin.field_3);
                    addNextInt16(ref thisData, thisCoin.x);
                    addNextInt16(ref thisData, thisCoin.y);
                }
                foreach(CoinEditStarCoin thisStarCoin in thisCourse.starCoins)
                {
                    thisData.Add(thisStarCoin.area);
                    thisData.Add(thisStarCoin.zone);
                    thisData.Add(thisStarCoin.field_2);
                    thisData.Add(thisStarCoin.field_3);
                    addNextFloat(ref thisData, thisStarCoin.x);
                    addNextFloat(ref thisData, thisStarCoin.y);
                }
            }
            for(int i = 0; i < 12; i++)
            {
                thisData.Add(this.coinEditData.unlocked[i]);
            }
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, coinEditDataSelectedIndex, thisData.Count)));
            
            /********************/
            /*playReportSettings*/
            /********************/
            int playReportSettingsStartIndex = thisData.Count;
            addNextInt32(ref thisData, this.playReportSettings.secondsPlayed);
            addNextInt32(ref thisData, this.playReportSettings.secondsInAdventure);
            addNextInt32(ref thisData, this.playReportSettings.nsmbuLevelsPlayed);
            addNextInt32(ref thisData, this.playReportSettings.singlePlayerLevels);
            addNextInt32(ref thisData, this.playReportSettings.twoPlayerLevels);
            addNextInt32(ref thisData, this.playReportSettings.threePlayerLevels);
            addNextInt32(ref thisData, this.playReportSettings.fourPlayerLevels);
            addNextInt32(ref thisData, this.playReportSettings.field_1C);
            addNextInt32(ref thisData, this.playReportSettings.numNabbitsChased);
            addNextInt32(ref thisData, this.playReportSettings.superGuidesSeen);
            addNextInt32(ref thisData, this.playReportSettings.boostRushesPlayed);
            addNextInt32(ref thisData, this.playReportSettings.boostRushOnePlayer);
            addNextInt32(ref thisData, this.playReportSettings.boostRushTwoPlayer);
            addNextInt32(ref thisData, this.playReportSettings.boostRushThreePlayer);
            addNextInt32(ref thisData, this.playReportSettings.boostRushFourPlayer);
            addNextInt32(ref thisData, this.playReportSettings.field_3C);
            addNextInt32(ref thisData, this.playReportSettings.field_40);
            addNextInt32(ref thisData, this.playReportSettings.numCoinBattlesPlayed);
            addNextInt32(ref thisData, this.playReportSettings.coinBattlesAlone);
            addNextInt32(ref thisData, this.playReportSettings.coinBattlesTwo);
            addNextInt32(ref thisData, this.playReportSettings.coinBattlesThree);
            addNextInt32(ref thisData, this.playReportSettings.coinBattlesFour);
            addNextInt32(ref thisData, this.playReportSettings.field_58);
            addNextInt32(ref thisData, this.playReportSettings.field_5C);
            addNextInt32(ref thisData, this.playReportSettings.numEditedCoinCoursesPlayed);
            addNextInt32(ref thisData, this.playReportSettings.numChallengesPlayed);
            addNextInt32(ref thisData, this.playReportSettings.field_68);
            addNextInt32(ref thisData, this.playReportSettings.numChallengeReplaysWatched);
            addNextInt32(ref thisData, this.playReportSettings.numBoostModeChallengesPlayed);
            addNextInt32(ref thisData, this.playReportSettings.nsluLevelsPlayed);
            addNextInt32(ref thisData, this.playReportSettings.field_78);
            addNextInt32(ref thisData, this.playReportSettings.field_7C);
            addNextInt32(ref thisData, this.playReportSettings.powerupsUsed);
            thisData.Add(this.playReportSettings.field_84);
            thisData.Add(this.playReportSettings.field_85);
            thisData.Add(this.playReportSettings.field_86);
            thisData.Add(this.playReportSettings.field_87);
            thisData.Add(this.playReportSettings.field_88);
            thisData.Add(this.playReportSettings.field_89);
            thisData.Add(this.playReportSettings.numGoldChallenges);
            thisData.Add(this.playReportSettings.numSilverChallenges);
            thisData.Add(this.playReportSettings.numBronzeChallenges);
            thisData.Add(this.playReportSettings.numCompletedBoostModeChallenges);
            thisData.Add(this.playReportSettings.field_8E);
            thisData.Add(this.playReportSettings.field_8F);
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, playReportSettingsStartIndex, thisData.Count)));
            
            /*************/
            /*miiSaveData*/
            /*************/
            int miiSaveDataStartIndex = thisData.Count;
            thisData.Add(this.miiSaveData.count);
            foreach(FFLCreateID thisFLLCreateID in this.miiSaveData.miiIdList)
            {
                thisData.Add(thisFLLCreateID.flags);
                thisData.Add(thisFLLCreateID.field_1);
                thisData.Add(thisFLLCreateID.field_2);
                thisData.Add(thisFLLCreateID.index);
                thisData.Add(thisFLLCreateID.createIdBase.field_0);
                thisData.Add(thisFLLCreateID.createIdBase.field_1);
                thisData.Add(thisFLLCreateID.createIdBase.field_2);
                thisData.Add(thisFLLCreateID.createIdBase.field_3);
                addNextInt16(ref thisData, thisFLLCreateID.createIdBase.authorIdCRC16);
            }
            thisData.Add(this.miiSaveData.field_26D);
            thisData.Add(this.miiSaveData.field_26E);
            thisData.Add(this.miiSaveData.field_26F);
            addNextInt32(ref thisData, Crc32Algorithm.Compute(sliceByteList(thisData, miiSaveDataStartIndex, thisData.Count)));

            /***********/
            /*Return it*/
            /***********/
            Console.WriteLine("Saved " + Convert.ToString(thisData.Count, 16) + " Bytes");
            return thisData;
        }

        public string byteToString(List<byte> byteToConvert)
        {
            string result = "";
            foreach(byte thisByte in byteToConvert)
            {
                result += thisByte.ToString("X2");
            }
            return result;
        }

        public byte[] sliceByteList(List<byte> listToSlice, int startIndex, int endIndex)
        {
            List<byte> result = new List<byte>();
            for(int i = startIndex; i < endIndex; i++)
            {
                result.Add(listToSlice[i]);
            }
            return result.ToArray();
        }

        public void addNextInt32(ref List<byte> data, uint intToAdd)
        {
            foreach (byte currentByte in BitConverter.GetBytes(intToAdd).Reverse<byte>())
            {
                data.Add(currentByte);
            }
        }

        public void addNextInt16(ref List<byte> data, ushort shortToAdd)
        {
            foreach (byte currentByte in BitConverter.GetBytes(shortToAdd).Reverse<byte>())
            {
                data.Add(currentByte);
            }
        }

        public void addNextFloat(ref List<byte> data, float floatToAdd)
        {
            foreach (byte currentByte in BitConverter.GetBytes(floatToAdd).Reverse<byte>())
            {
                data.Add(currentByte);
            }
        }

        public void loadFile(string path, ref List<byte> rawdata)
        {
            //Reads the Save File, this little part was done by Lory some months ago for one of my older projects, I reused it for this tool as it works good. Thanks to him !
            FileStream rawSave = File.Open(path, FileMode.Open);
            try
            {
                BinaryReader binReader = new BinaryReader(rawSave);
                byte b = binReader.ReadByte();
                while (b != null)
                {
                    rawdata.Add(b);
                    b = binReader.ReadByte();
                }
                binReader.Close();
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine("reached end of stream");
                rawSave.Close();
            }

            //make it a string and an int for future things
            char[] rawstring = Encoding.UTF8.GetString(rawdata.ToArray(), 0, rawdata.ToArray().Length).ToCharArray();
            uint[] rawint = new uint[rawdata.ToArray().Length];
            for (int i = 0; i < rawdata.ToArray().Length - 1; i++)
            {
                rawint[i] = Convert.ToUInt32(rawdata[i].ToString());
            }

            //Checks the header
            if (rawstring[0] != 'R' || rawstring[1] != 'P' || rawstring[2] != 'S' || rawstring[3] != 'D')
            {
                MessageBox.Show("Invalid Save File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //Here the fun starts (or not)

                /********/
                /*header*/
                /********/
                int index = 0;
                SaveDataHeader header = new SaveDataHeader();

                header.header = getNextInt32(ref index, rawdata);
                header.always1 = rawdata[index++];
                header.always0 = rawdata[index++];
                header.always2 = rawdata[index++];
                header.fileIndex = rawdata[index++];
                header.hasBeenNotifiedRDash = rawdata[index++];
                header.buttonLayout = rawdata[index++];
                header.field_A = rawdata[index++];
                header.field_B = rawdata[index++];
                header.CRC32 = getNextInt32(ref index, rawdata);

                /*************************/
                /*profiles & tempProfiles*/
                /*************************/
                List<ProfileSaveData> profiles = new List<ProfileSaveData>();
                for (int i = 0; i < 12; i++)
                {
                    ProfileSaveData thisProfile = new ProfileSaveData();

                    thisProfile.isValid = rawdata[index++];
                    thisProfile.lastNumPlayers = rawdata[index++];
                    thisProfile.coins = rawdata[index++];
                    thisProfile.rockWorldSwitchState = rawdata[index++];

                    for (int j = 0; j < 2; j++)
                    {
                        SaveCSLocation thisSaveCSLocation = new SaveCSLocation();

                        thisSaveCSLocation.world = rawdata[index++];
                        thisSaveCSLocation.section = rawdata[index++];
                        thisSaveCSLocation.node = rawdata[index++];

                        if (j == 0) { thisProfile.playerLoc1 = thisSaveCSLocation; }
                        else { thisProfile.playerLoc2 = thisSaveCSLocation; }
                    }

                    thisProfile.field_A = rawdata[index++];
                    thisProfile.field_B = rawdata[index++];

                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.lives.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.characters.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.powerups.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.continues.Add(rawdata[index++]);
                    }

                    thisProfile.worldsBeatenFlags = getNextInt16(ref index, rawdata);

                    thisProfile.field_1E = rawdata[index++];
                    thisProfile.field_1F = rawdata[index++];

                    thisProfile.vec3.x = getNextFloat(ref index, rawdata);
                    thisProfile.vec3.y = getNextFloat(ref index, rawdata);
                    thisProfile.vec3.z = getNextFloat(ref index, rawdata);

                    thisProfile.field_2C = rawdata[index++];
                    thisProfile.field_2D = rawdata[index++];
                    thisProfile.gameCompletionFlags = rawdata[index++];
                    thisProfile.field_2F = rawdata[index++];
                    thisProfile.score = getNextInt32(ref index, rawdata);

                    for (int j = 0; j < 16; j++)
                    {
                        thisProfile.field_34.Add(rawdata[index++]);
                    }
                    thisProfile.bestCreditsCoins = getNextInt16(ref index, rawdata);

                    thisProfile.airshipLocation.world = rawdata[index++];
                    thisProfile.airshipLocation.section = rawdata[index++];
                    thisProfile.airshipLocation.node = rawdata[index++];

                    thisProfile.nabbitWorld = rawdata[index++];
                    thisProfile.nabbitWorld2 = rawdata[index++];
                    thisProfile.field_4B = rawdata[index++];
                    thisProfile.field_4C = rawdata[index++];
                    thisProfile.field_4D = rawdata[index++];
                    thisProfile.babyYoshiItem = rawdata[index++];
                    thisProfile.field_4F = rawdata[index++];
                    thisProfile.field_50 = rawdata[index++];
                    thisProfile.field_51 = rawdata[index++];
                    thisProfile.field_52 = rawdata[index++];
                    thisProfile.field_53 = rawdata[index++];
                    thisProfile.field_54 = rawdata[index++];
                    thisProfile.availableBabyYoshiFlags = rawdata[index++];
                    thisProfile.activeBabyYoshi = rawdata[index++];
                    thisProfile.field_57 = rawdata[index++];
                    thisProfile.field_58 = rawdata[index++];
                    thisProfile.field_59 = rawdata[index++];
                    thisProfile.levelCounterForBalloonYoshi = rawdata[index++];
                    thisProfile.levelCounterForBubbleYoshi = rawdata[index++];

                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.ambushItemCounters.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        thisProfile.ambushItems.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 7; j++)
                    {
                        thisProfile.remainingNabbitAttempts.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 123; j++)
                    {
                        thisProfile.levelFlags.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 62; j++)
                    {
                        thisProfile.levelDeathCounters.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 41; j++)
                    {
                        thisProfile.starCoinCollection.Add(rawdata[index++]);
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        thisProfile.inventoryItems.Add(rawdata[index++]);
                    }

                    List<AmbushSaveEnemy> thisAmbushSaveEnemyList = new List<AmbushSaveEnemy>();
                    for (int j = 0; j < 18; j++)
                    {
                        AmbushSaveEnemy thisAmbushSaveEnemy = new AmbushSaveEnemy();

                        thisAmbushSaveEnemy.location.world = rawdata[index++];
                        thisAmbushSaveEnemy.location.section = rawdata[index++];
                        thisAmbushSaveEnemy.location.node = rawdata[index++];
                        thisAmbushSaveEnemy.field_3 = rawdata[index++];
                        thisAmbushSaveEnemy.field_4 = rawdata[index++];

                        thisAmbushSaveEnemyList.Add(thisAmbushSaveEnemy);
                    }
                    thisProfile.ambushState.world1.Add(thisAmbushSaveEnemyList[0]);
                    thisProfile.ambushState.world1.Add(thisAmbushSaveEnemyList[1]);
                    thisProfile.ambushState.world2.Add(thisAmbushSaveEnemyList[2]);
                    thisProfile.ambushState.world2.Add(thisAmbushSaveEnemyList[3]);
                    thisProfile.ambushState.world2.Add(thisAmbushSaveEnemyList[4]);
                    thisProfile.ambushState.world3.Add(thisAmbushSaveEnemyList[5]);
                    thisProfile.ambushState.world3.Add(thisAmbushSaveEnemyList[6]);
                    thisProfile.ambushState.world4.Add(thisAmbushSaveEnemyList[7]);
                    thisProfile.ambushState.world4.Add(thisAmbushSaveEnemyList[8]);
                    thisProfile.ambushState.world4.Add(thisAmbushSaveEnemyList[9]);
                    thisProfile.ambushState.world4.Add(thisAmbushSaveEnemyList[10]);
                    thisProfile.ambushState.world4.Add(thisAmbushSaveEnemyList[11]);
                    thisProfile.ambushState.world5.Add(thisAmbushSaveEnemyList[13]);
                    thisProfile.ambushState.world5.Add(thisAmbushSaveEnemyList[13]);
                    thisProfile.ambushState.world5.Add(thisAmbushSaveEnemyList[14]);
                    thisProfile.ambushState.world7.Add(thisAmbushSaveEnemyList[15]);
                    thisProfile.ambushState.world7.Add(thisAmbushSaveEnemyList[16]);
                    thisProfile.ambushState.world7.Add(thisAmbushSaveEnemyList[17]);
                    for (int j = 0; j < 18; j++)
                    {
                        thisProfile.ambushState.field_5A.Add(rawdata[index++]);
                    }
                    thisProfile.ambushState.field_6C = rawdata[index++];


                    thisProfile.totalStats.totalCoins = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.totalStarCoins = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.goals = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.clappings = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.boostBlockDistance = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.miniBoostBlockDistance = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.caughtNabbits = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.stompedGoombas = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.collectedItems = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.goalsWithYoshi = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.goalsWithBabyYoshi = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.goalOneUps = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.goalFireworks = getNextInt32(ref index, rawdata);
                    thisProfile.totalStats.threeUpMoons = getNextInt32(ref index, rawdata);

                    thisProfile.caughtNabbitFlags = rawdata[index++];
                    for (int j = 0; j < 3; j++)
                    {
                        thisProfile.padding.Add(rawdata[index++]);
                    }
                    thisProfile.CRC32 = getNextInt32(ref index, rawdata);


                    profiles.Add(thisProfile);
                }

                /***************/
                /*challengeData*/
                /***************/
                ChallengeSaveData challengeData = new ChallengeSaveData();
                challengeData.initialized = rawdata[index++];
                for (int i = 0; i < 5; i++)
                {
                    challengeData.categoryMedals.Add(rawdata[index++]);
                }
                challengeData.field_6 = rawdata[index++];
                challengeData.field_7 = rawdata[index++];
                for (int i = 0; i < 80; i++)
                {
                    ChallengeSave thisChallengeSave = new ChallengeSave();

                    thisChallengeSave.state = rawdata[index++];
                    thisChallengeSave.player = rawdata[index++];
                    thisChallengeSave.field_2 = rawdata[index++];
                    thisChallengeSave.field_3 = rawdata[index++];

                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_0 = getNextInt32(ref index, rawdata);

                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_0 = getNextInt16(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_2 = getNextInt16(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_4 = getNextInt16(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.authorID.field_6 = getNextInt16(ref index, rawdata);

                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.flags = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.field_1 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.field_2 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.index = rawdata[index++];

                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_0 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_1 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_2 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.field_3 = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.createId.createIdBase.authorIdCRC16 = getNextInt16(ref index, rawdata);

                    for (int j = 0; j < 2; j++)
                    {
                        thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_16.Add(rawdata[index++]);
                    }
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_18 = getNextInt16(ref index, rawdata);

                    for (int j = 0; j < 10; j++)
                    {
                        thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.miiName.Add(getNextInt16(ref index, rawdata));
                    }
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_2E = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_2F = rawdata[index++];
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_30 = getNextInt32(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_34 = getNextInt32(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_38 = getNextInt32(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_3C = getNextInt32(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_40 = getNextInt32(ref index, rawdata);
                    thisChallengeSave.fflStoreData.miiDataOfficial.miiDataCore.field_44 = getNextInt32(ref index, rawdata);
                    for (int j = 0; j < 10; j++)
                    {
                        thisChallengeSave.fflStoreData.miiDataOfficial.creatorName.Add(getNextInt16(ref index, rawdata));
                    }
                    thisChallengeSave.fflStoreData.field_5C = getNextInt16(ref index, rawdata);
                    thisChallengeSave.fflStoreData.CRC16 = getNextInt16(ref index, rawdata);

                    thisChallengeSave.timer = getNextInt32(ref index, rawdata);
                    thisChallengeSave.replayCRC32 = getNextInt32(ref index, rawdata);


                    challengeData.challenges.Add(thisChallengeSave);
                }
                for (int i = 0; i < 5184; i++)
                {
                    challengeData.field_21C8.Add(rawdata[index++]);
                }
                challengeData.CRC32 = getNextInt32(ref index, rawdata);

                /***************/
                /*boostRushData*/
                /***************/
                BoostRushSaveData boostRushData = new BoostRushSaveData();
                boostRushData.field_0 = rawdata[index++];
                boostRushData.field_1 = rawdata[index++];
                boostRushData.field_2 = rawdata[index++];
                boostRushData.field_3 = rawdata[index++];
                for (int i = 0; i < 32; i++)
                {
                    boostRushData.secondHundreds.Add(getNextInt32(ref index, rawdata));
                }
                for (int i = 0; i < 32; i++)
                {
                    boostRushData.field_84.Add(rawdata[index++]);
                }
                boostRushData.CRC32 = getNextInt32(ref index, rawdata);

                /**************/
                /*coinEditData*/
                /**************/
                CoinEditSaveData coinEditData = new CoinEditSaveData();
                for (int i = 0; i < 10; i++)
                {
                    CoinEditPatternStage thisCoinEditPatternStage = new CoinEditPatternStage();
                    for (int j = 0; j < 300; j++)
                    {
                        CoinEditCoin thisCoinEditCoin = new CoinEditCoin();

                        thisCoinEditCoin.area = rawdata[index++];
                        thisCoinEditCoin.zone = rawdata[index++];
                        thisCoinEditCoin.isValid = rawdata[index++];
                        thisCoinEditCoin.field_3 = rawdata[index++];
                        thisCoinEditCoin.x = getNextInt16(ref index, rawdata);
                        thisCoinEditCoin.y = getNextInt16(ref index, rawdata);

                        thisCoinEditPatternStage.coins.Add(thisCoinEditCoin);
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        CoinEditStarCoin thisCoinEditStarCoin = new CoinEditStarCoin();

                        thisCoinEditStarCoin.area = rawdata[index++];
                        thisCoinEditStarCoin.zone = rawdata[index++];
                        thisCoinEditStarCoin.field_2 = rawdata[index++];
                        thisCoinEditStarCoin.field_3 = rawdata[index++];
                        thisCoinEditStarCoin.x = getNextFloat(ref index, rawdata);
                        thisCoinEditStarCoin.y = getNextFloat(ref index, rawdata);

                        thisCoinEditPatternStage.starCoins.Add(thisCoinEditStarCoin);
                    }
                    coinEditData.courses.Add(thisCoinEditPatternStage);
                }
                for (int i = 0; i < 12; i++)
                {
                    coinEditData.unlocked.Add(rawdata[index++]);
                }
                coinEditData.CRC32 = getNextInt32(ref index, rawdata);

                /********************/
                /*playReportSettings*/
                /********************/
                PlayReportSettings playReportSettings = new PlayReportSettings();
                playReportSettings.secondsPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.secondsInAdventure = getNextInt32(ref index, rawdata);
                playReportSettings.nsmbuLevelsPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.singlePlayerLevels = getNextInt32(ref index, rawdata);
                playReportSettings.twoPlayerLevels = getNextInt32(ref index, rawdata);
                playReportSettings.threePlayerLevels = getNextInt32(ref index, rawdata);
                playReportSettings.fourPlayerLevels = getNextInt32(ref index, rawdata);
                playReportSettings.field_1C = getNextInt32(ref index, rawdata);
                playReportSettings.numNabbitsChased = getNextInt32(ref index, rawdata);
                playReportSettings.superGuidesSeen = getNextInt32(ref index, rawdata);
                playReportSettings.boostRushesPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.boostRushOnePlayer = getNextInt32(ref index, rawdata);
                playReportSettings.boostRushTwoPlayer = getNextInt32(ref index, rawdata);
                playReportSettings.boostRushThreePlayer = getNextInt32(ref index, rawdata);
                playReportSettings.boostRushFourPlayer = getNextInt32(ref index, rawdata);
                playReportSettings.field_3C = getNextInt32(ref index, rawdata);
                playReportSettings.field_40 = getNextInt32(ref index, rawdata);
                playReportSettings.numCoinBattlesPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.coinBattlesAlone = getNextInt32(ref index, rawdata);
                playReportSettings.coinBattlesTwo = getNextInt32(ref index, rawdata);
                playReportSettings.coinBattlesThree = getNextInt32(ref index, rawdata);
                playReportSettings.coinBattlesFour = getNextInt32(ref index, rawdata);
                playReportSettings.field_58 = getNextInt32(ref index, rawdata);
                playReportSettings.field_5C = getNextInt32(ref index, rawdata);
                playReportSettings.numEditedCoinCoursesPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.numChallengesPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.field_68 = getNextInt32(ref index, rawdata);
                playReportSettings.numChallengeReplaysWatched = getNextInt32(ref index, rawdata);
                playReportSettings.numBoostModeChallengesPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.nsluLevelsPlayed = getNextInt32(ref index, rawdata);
                playReportSettings.field_78 = getNextInt32(ref index, rawdata);
                playReportSettings.field_7C = getNextInt32(ref index, rawdata);
                playReportSettings.powerupsUsed = getNextInt32(ref index, rawdata);
                playReportSettings.field_84 = rawdata[index++];
                playReportSettings.field_85 = rawdata[index++];
                playReportSettings.field_86 = rawdata[index++];
                playReportSettings.field_87 = rawdata[index++];
                playReportSettings.field_88 = rawdata[index++];
                playReportSettings.field_89 = rawdata[index++];
                playReportSettings.numGoldChallenges = rawdata[index++];
                playReportSettings.numSilverChallenges = rawdata[index++];
                playReportSettings.numBronzeChallenges = rawdata[index++];
                playReportSettings.numCompletedBoostModeChallenges = rawdata[index++];
                playReportSettings.field_8E = rawdata[index++];
                playReportSettings.field_8F = rawdata[index++];
                playReportSettings.CRC32 = getNextInt32(ref index, rawdata);

                /*************/
                /*miiSaveData*/
                /*************/
                MiiSaveData miiSaveData = new MiiSaveData();
                miiSaveData.count = rawdata[index++];
                for (int i = 0; i < 62; i++)
                {
                    FFLCreateID thisFFLCreateID = new FFLCreateID();

                    thisFFLCreateID.flags = rawdata[index++];
                    thisFFLCreateID.field_1 = rawdata[index++];
                    thisFFLCreateID.field_2 = rawdata[index++];
                    thisFFLCreateID.index = rawdata[index++];
                    thisFFLCreateID.createIdBase.field_0 = rawdata[index++];
                    thisFFLCreateID.createIdBase.field_1 = rawdata[index++];
                    thisFFLCreateID.createIdBase.field_2 = rawdata[index++];
                    thisFFLCreateID.createIdBase.field_3 = rawdata[index++];
                    thisFFLCreateID.createIdBase.authorIdCRC16 = getNextInt16(ref index, rawdata);

                    miiSaveData.miiIdList.Add(thisFFLCreateID);
                }
                miiSaveData.field_26D = rawdata[index++];
                miiSaveData.field_26E = rawdata[index++];
                miiSaveData.field_26F = rawdata[index++];
                miiSaveData.CRC32 = getNextInt32(ref index, rawdata);

                Console.WriteLine("miiSaveData: " + Convert.ToString(index, 16));

                Console.WriteLine("Read " + Convert.ToString(index, 16) + " Bytes");

                /****************************/
                /*Add everything to thisSave*/
                /****************************/

                this.header = header;
                this.profiles = profiles;
                this.challengeData = challengeData;
                this.boostRushData = boostRushData;
                this.coinEditData = coinEditData;
                this.playReportSettings = playReportSettings;
                this.miiSaveData = miiSaveData;
            }
        }

        public uint getNextInt32(ref int index, List<byte> rawdata)
        {
            byte[] nextInt32 = { rawdata[index++], rawdata[index++], rawdata[index++], rawdata[index++] };
            Array.Reverse(nextInt32);
            return BitConverter.ToUInt32(nextInt32, 0);
        }

        public ushort getNextInt16(ref int index, List<byte> rawdata)
        {
            byte[] nextInt16 = { rawdata[index++], rawdata[index++] };
            Array.Reverse(nextInt16);
            return BitConverter.ToUInt16(nextInt16, 0);
        }

        public float getNextFloat(ref int index, List<byte> rawdata)
        {
            byte[] nextInt32 = { rawdata[index++], rawdata[index++], rawdata[index++], rawdata[index++] };
            Array.Reverse(nextInt32);
            return BitConverter.ToSingle(nextInt32, 0);
        }


        internal SaveDataHeader header = new SaveDataHeader();
        internal List<ProfileSaveData> profiles = new List<ProfileSaveData>();          //12    //Listen, I'd LOVE to use an array instead of a list, but for some reason, C# refuses :c
        //internal List<ProfileSaveData> tempProfiles = new List<ProfileSaveData>();      //6
        internal ChallengeSaveData challengeData = new ChallengeSaveData();
        internal BoostRushSaveData boostRushData = new BoostRushSaveData();
        internal CoinEditSaveData coinEditData = new CoinEditSaveData();
        internal PlayReportSettings playReportSettings = new PlayReportSettings();
        internal MiiSaveData miiSaveData = new MiiSaveData();
    }

    public class SaveDataHeader
    {
        internal uint header = new uint();
        internal byte always1 = new byte();
        internal byte always0 = new byte();
        internal byte always2 = new byte();
        internal byte fileIndex = new byte();
        internal byte hasBeenNotifiedRDash = new byte();
        internal byte buttonLayout = new byte();
        internal byte field_A = new byte();
        internal byte field_B = new byte();
        internal uint CRC32 = new uint();
    }

    public class ProfileSaveData
    {
        internal byte isValid = new byte();
        internal byte lastNumPlayers = new byte();
        internal byte coins = new byte();
        internal byte rockWorldSwitchState = new byte();
        internal SaveCSLocation playerLoc1 = new SaveCSLocation();
        internal SaveCSLocation playerLoc2 = new SaveCSLocation();
        internal byte field_A = new byte();
        internal byte field_B = new byte();
        internal List<byte> lives = new List<byte>();                                   //4
        internal List<byte> characters = new List<byte>();                              //4
        internal List<byte> powerups = new List<byte>();                                //4
        internal List<byte> continues = new List<byte>();                               //4
        internal ushort worldsBeatenFlags = new ushort();
        internal byte field_1E = new byte();
        internal byte field_1F = new byte();
        internal Vec3 vec3 = new Vec3();
        internal byte field_2C = new byte();
        internal byte field_2D = new byte();
        internal byte gameCompletionFlags = new byte();
        internal byte field_2F = new byte();
        internal uint score = new uint();
        internal List<byte> field_34 = new List<byte>();                                //16
        internal ushort bestCreditsCoins = new ushort();
        internal SaveCSLocation airshipLocation = new SaveCSLocation();
        internal byte nabbitWorld = new byte();
        internal byte nabbitWorld2 = new byte();
        internal byte field_4B = new byte();
        internal byte field_4C = new byte();
        internal byte field_4D = new byte();
        internal byte babyYoshiItem = new byte();
        internal byte field_4F = new byte();
        internal byte field_50 = new byte();
        internal byte field_51 = new byte();
        internal byte field_52 = new byte();
        internal byte field_53 = new byte();
        internal byte field_54 = new byte();
        internal byte availableBabyYoshiFlags = new byte();
        internal byte activeBabyYoshi = new byte();
        internal byte field_57 = new byte();
        internal byte field_58 = new byte();
        internal byte field_59 = new byte();
        internal byte levelCounterForBalloonYoshi = new byte();
        internal byte levelCounterForBubbleYoshi = new byte();
        internal List<byte> ambushItemCounters = new List<byte>();                      //4
        internal List<byte> ambushItems = new List<byte>();                             //4
        internal List<byte> remainingNabbitAttempts = new List<byte>();                 //7
        internal List<byte> levelFlags = new List<byte>();                              //123
        internal List<byte> levelDeathCounters = new List<byte>();                      //62
        internal List<byte> starCoinCollection = new List<byte>();                      //41
        internal List<byte> inventoryItems = new List<byte>();                          //10
        internal AmbushSaveState ambushState = new AmbushSaveState();
        internal TotalStats totalStats = new TotalStats();
        internal byte caughtNabbitFlags = new byte();
        internal List<byte> padding = new List<byte>();                                 //3
        internal uint CRC32 = new uint();
    }

    public class SaveCSLocation
    {
        internal byte world = new byte();
        internal byte section = new byte();
        internal byte node = new byte();
    }

    public class Vec3
    {
        internal float x = new float();
        internal float y = new float();
        internal float z = new float();
    }

    public class AmbushSaveState
    {
        internal List<AmbushSaveEnemy> world1 = new List<AmbushSaveEnemy>();            //2
        internal List<AmbushSaveEnemy> world2 = new List<AmbushSaveEnemy>();            //3
        internal List<AmbushSaveEnemy> world3 = new List<AmbushSaveEnemy>();            //2
        internal List<AmbushSaveEnemy> world4 = new List<AmbushSaveEnemy>();            //5
        internal List<AmbushSaveEnemy> world5 = new List<AmbushSaveEnemy>();            //3
        internal List<AmbushSaveEnemy> world7 = new List<AmbushSaveEnemy>();            //3
        internal List<byte> field_5A = new List<byte>();                                //18
        internal byte field_6C = new byte();
    }

    public class AmbushSaveEnemy
    {
        internal SaveCSLocation location = new SaveCSLocation();
        internal byte field_3 = new byte();
        internal byte field_4 = new byte();
    }

    public class TotalStats
    {
        internal uint totalCoins = new uint();
        internal uint totalStarCoins = new uint();
        internal uint goals = new uint();
        internal uint clappings = new uint();
        internal uint boostBlockDistance = new uint();
        internal uint miniBoostBlockDistance = new uint();
        internal uint caughtNabbits = new uint();
        internal uint stompedGoombas = new uint();
        internal uint collectedItems = new uint();
        internal uint goalsWithYoshi = new uint();
        internal uint goalsWithBabyYoshi = new uint();
        internal uint goalOneUps = new uint();
        internal uint goalFireworks = new uint();
        internal uint threeUpMoons = new uint();
    }

    public class ChallengeSaveData
    {
        internal byte initialized = new byte();
        internal List<byte> categoryMedals = new List<byte>();                          //5
        internal byte field_6 = new byte();
        internal byte field_7 = new byte();
        internal List<ChallengeSave> challenges = new List<ChallengeSave>();            //80
        internal List<byte> field_21C8 = new List<byte>();                              //5184
        internal uint CRC32 = new uint();
    }

    public class ChallengeSave
    {
        internal byte state = new byte();
        internal byte player = new byte();
        internal byte field_2 = new byte();
        internal byte field_3 = new byte();
        internal FFLStoreData fflStoreData = new FFLStoreData();
        internal uint timer = new uint();
        internal uint replayCRC32 = new uint();
    }

    public class FFLStoreData
    {
        internal FFLiMiiDataOfficial miiDataOfficial = new FFLiMiiDataOfficial();
        internal ushort field_5C = new ushort();
        internal ushort CRC16 = new ushort();
    }

    public class FFLiMiiDataOfficial
    {
        internal FFLiMiiDataCore miiDataCore = new FFLiMiiDataCore();
        internal List<ushort> creatorName = new List<ushort>();                         //10
    }

    public class FFLiMiiDataCore
    {
        internal uint field_0 = new uint();
        internal FFLiAuthorID authorID = new FFLiAuthorID();
        internal FFLCreateID createId = new FFLCreateID();
        internal List<byte> field_16 = new List<byte>();
        internal ushort field_18 = new ushort();
        internal List<ushort> miiName = new List<ushort>();                             //10
        internal byte field_2E = new byte();
        internal byte field_2F = new byte();
        internal uint field_30 = new uint();
        internal uint field_34 = new uint();
        internal uint field_38 = new uint();
        internal uint field_3C = new uint();
        internal uint field_40 = new uint();
        internal uint field_44 = new uint();
    }

    public class FFLiAuthorID
    {
        internal ushort field_0 = new ushort();
        internal ushort field_2 = new ushort();
        internal ushort field_4 = new ushort();
        internal ushort field_6 = new ushort();
    }

    public class FFLCreateID
    {
        internal byte flags = new byte();
        internal byte field_1 = new byte();
        internal byte field_2 = new byte();
        internal byte index = new byte();
        internal FFLiCreateIDBase createIdBase = new FFLiCreateIDBase();
    }

    public class FFLiCreateIDBase
    {
        internal byte field_0 = new byte();
        internal byte field_1 = new byte();
        internal byte field_2 = new byte();
        internal byte field_3 = new byte();
        internal ushort authorIdCRC16 = new ushort();
    }

    public class BoostRushSaveData
    {
        internal byte field_0 = new byte();
        internal byte field_1 = new byte();
        internal byte field_2 = new byte();
        internal byte field_3 = new byte();
        internal List<uint> secondHundreds = new List<uint>();                      //32
        internal List<byte> field_84 = new List<byte>();                                //32
        internal uint CRC32 = new uint();
    }

    public class CoinEditSaveData
    {
        internal List<CoinEditPatternStage> courses = new List<CoinEditPatternStage>(); //10
        internal List<byte> unlocked = new List<byte>();                                //12
        internal uint CRC32 = new uint();
    }

    public class CoinEditPatternStage
    {
        internal List<CoinEditCoin> coins = new List<CoinEditCoin>();                   //300
        internal List<CoinEditStarCoin> starCoins = new List<CoinEditStarCoin>();       //3
    }

    public class CoinEditCoin
    {
        internal byte area = new byte();
        internal byte zone = new byte();
        internal byte isValid = new byte();
        internal byte field_3 = new byte();
        internal ushort x = new ushort();
        internal ushort y = new ushort();
    }

    public class CoinEditStarCoin
    {
        internal byte area = new byte();
        internal byte zone = new byte();
        internal byte field_2 = new byte();
        internal byte field_3 = new byte();
        internal float x = new float();
        internal float y = new float();
    }

    public class PlayReportSettings
    {
        internal uint secondsPlayed = new uint();
        internal uint secondsInAdventure = new uint();
        internal uint nsmbuLevelsPlayed = new uint();
        internal uint singlePlayerLevels = new uint();
        internal uint twoPlayerLevels = new uint();
        internal uint threePlayerLevels = new uint();
        internal uint fourPlayerLevels = new uint();
        internal uint field_1C = new uint();
        internal uint numNabbitsChased = new uint();
        internal uint superGuidesSeen = new uint();
        internal uint boostRushesPlayed = new uint();
        internal uint boostRushOnePlayer = new uint();
        internal uint boostRushTwoPlayer = new uint();
        internal uint boostRushThreePlayer = new uint();
        internal uint boostRushFourPlayer = new uint();
        internal uint field_3C = new uint();
        internal uint field_40 = new uint();
        internal uint numCoinBattlesPlayed = new uint();
        internal uint coinBattlesAlone = new uint();
        internal uint coinBattlesTwo = new uint();
        internal uint coinBattlesThree = new uint();
        internal uint coinBattlesFour = new uint();
        internal uint field_58 = new uint();
        internal uint field_5C = new uint();
        internal uint numEditedCoinCoursesPlayed = new uint();
        internal uint numChallengesPlayed = new uint();
        internal uint field_68 = new uint();
        internal uint numChallengeReplaysWatched = new uint();
        internal uint numBoostModeChallengesPlayed = new uint();
        internal uint nsluLevelsPlayed = new uint();
        internal uint field_78 = new uint();
        internal uint field_7C = new uint();
        internal uint powerupsUsed = new uint();
        internal byte field_84 = new byte();
        internal byte field_85 = new byte();
        internal byte field_86 = new byte();
        internal byte field_87 = new byte();
        internal byte field_88 = new byte();
        internal byte field_89 = new byte();
        internal byte numGoldChallenges = new byte();
        internal byte numSilverChallenges = new byte();
        internal byte numBronzeChallenges = new byte();
        internal byte numCompletedBoostModeChallenges = new byte();
        internal byte field_8E = new byte();
        internal byte field_8F = new byte();
        internal uint CRC32 = new uint();
    }

    public class MiiSaveData
    {
        internal byte count = new byte();
        internal List<FFLCreateID> miiIdList = new List<FFLCreateID>();                 //62
        internal byte field_26D = new byte();
        internal byte field_26E = new byte();
        internal byte field_26F = new byte();
        internal uint CRC32 = new uint();
    }
}
