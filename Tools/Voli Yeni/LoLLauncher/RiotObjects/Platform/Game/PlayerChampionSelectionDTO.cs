namespace LoLLauncher.RiotObjects.Platform.Game
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects;
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerChampionSelectionDTO : RiotGamesObject
    {
        private Callback callback;
        private string type;

        public PlayerChampionSelectionDTO()
        {
            this.type = "com.riotgames.platform.game.PlayerChampionSelectionDTO";
        }

        public PlayerChampionSelectionDTO(Callback callback)
        {
            this.type = "com.riotgames.platform.game.PlayerChampionSelectionDTO";
            this.callback = callback;
        }

        public PlayerChampionSelectionDTO(TypedObject result)
        {
            this.type = "com.riotgames.platform.game.PlayerChampionSelectionDTO";
            base.SetFields<PlayerChampionSelectionDTO>(this, result);
        }

        public override void DoCallback(TypedObject result)
        {
            base.SetFields<PlayerChampionSelectionDTO>(this, result);
            this.callback(this);
        }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("selectedSkinIndex")]
        public int SelectedSkinIndex { get; set; }

        [InternalName("spell1Id")]
        public double Spell1Id { get; set; }

        [InternalName("spell2Id")]
        public double Spell2Id { get; set; }

        [InternalName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        public delegate void Callback(PlayerChampionSelectionDTO result);
    }
}

