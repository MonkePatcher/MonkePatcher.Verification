using QuestPatcher.QMod;
using System.IO.Compression;
using System.Security.Cryptography;

namespace MonkePatcher.Verification
{
    public class QModVerifier
    {
        private Dictionary<string, ModHashes>? hashes;

        private QMod qmod;

        public QModVerifier(QMod qmod)
        {
            this.qmod = qmod;
        }

        public Dictionary<string, ModHashes> Hashes
        {
            get
            {
                if (hashes == null)
                {
                    hashes = ModHashes.GetHashes();
                }

// Disabling because we check if hashes is null
#pragma warning disable CS8603 // Possible null reference return.
                return hashes;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        /// <summary>
        /// Verifies if the mod matches the verified hashes
        /// </summary>
        /// <returns><c>true</c> if the QMod matches known hashes, <c>false</c> otherwise</returns>
        public bool Verify()
        {
            string id = qmod.Id;

            string? verifiedHash = Hashes[id].QModHash;

            if (verifiedHash == null) return false;

            using var md5 = MD5.Create();

            
        }
    }
}
