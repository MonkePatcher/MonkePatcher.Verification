using QuestPatcher.QMod;
using System.IO.Compression;
using System.Security.Cryptography;

namespace MonkePatcher.Verification
{
    public class Verifier
    {
        private Dictionary<string, ModHashes>? hashes;

        private QMod qmod;

        public Verifier(QMod qmod)
        {
            this.qmod = qmod;
        }

        public static string QModVerificationPath
        {
            get => Path.Combine(Path.GetTempPath(), "MonkePatcher", "Verification");
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
        public bool VerifyQMod()
        {
            
            // I'm not a fan of this code. I can't get a stream from a ZipArchive
            // so I had to write this monstrosity

            string id = qmod.Id;

            string tempPath = Path.Combine(QModVerificationPath, id);

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            qmod.Archive.ExtractToDirectory(tempPath);

            string qmodTempArchive = Path.Combine(QModVerificationPath, $"{id}.qmod");
            ZipFile.CreateFromDirectory(tempPath, qmodTempArchive);

            FileStream fs = File.OpenRead(qmodTempArchive);

            string? verifiedHash = Hashes[id].QModHash;
            if (verifiedHash == null) return false;

            using MD5 md5 = MD5.Create();
            string? qmodHash = md5.ComputeHash(fs).ToString();

            return qmodHash == verifiedHash;
        }

        public bool VerifySO(string soName)
        {
            var entry = qmod.Archive.GetEntry(soName);
            if (entry == null) return false;

            Stream soStream = entry.Open();

            string? verifiedHash = Hashes[qmod.Id].SOHash;
            if (verifiedHash == null) return false;

            using MD5 md5 = MD5.Create();
            string? soHash = md5.ComputeHash(soStream).ToString();

            return soHash == verifiedHash;
        }
    }
}
