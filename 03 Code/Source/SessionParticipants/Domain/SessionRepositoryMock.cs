using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#pragma warning disable 1998

namespace SessionParticipants.Domain
{
    public class SessionRepositoryMock : ISessionRepository
    {
        public SessionRepositoryMock()
        {
        }
        public async Task<List<Session>> GetSessionsAsync(TimeSpan unused)
        {
            var sessions = new List<Session>();
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 0,
                Title = "Mock #nossued Haupt Konferenzraum",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 1,
                Title = "Mock #nossued Hauptkonferenzraum",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 2,
                Title = "Mock #nossued Track1",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 3,
                Title = "Mock #nossued Track2",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 4,
                Title = "Mock #nossued Track3",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 5,
                Title = "Mock #nossued Terrasse",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 6,
                Title = "Mock #nossued Café",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            sessions.Add(new Session
            {
                Id = Guid.NewGuid().ToString().Substring(0, 10),
                SortOrder = 7,
                Title = "Mock #nossued Flur",
                ParticipationUrl = new Uri("https://www.nossued.de")
            });
            var rnd = new Random();
            var allParticipants = GetAllParticipants();
            Shuffle(allParticipants, rnd);
            
            foreach (var session in sessions)
            {
                if (allParticipants.Count > 4)
                {
                    var amount = rnd.Next(4, allParticipants.Count);
                    session.Participants.AddRange(allParticipants.GetRange(0, amount));
                    allParticipants.RemoveRange(0, amount);
                }
            }
            return sessions;
        }
        private List<Participant> GetAllParticipants()
        {
            var participants = new List<Participant>();

            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Enyonam Reiner" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Majda" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Björn" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Ksawery" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Sibylle Ignatios" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Alwine" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Gottlieb" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Radomił Ahmadu" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Sauda" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Anita Edi" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Vreni" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Idir" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Lew" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Lenz" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Vinka" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Gerhild" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Emmy" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Kurt Wieland" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Lothar" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Pemphero" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Małgosia" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Pelagia Apostolos" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Achieng" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Miško" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Nicolaos" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Walther" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Thorsten" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Hendrik" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Katarine" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Aniela" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Alemayehu" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Venetia" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Bożydar" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Emilie Sibongile" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Bernard" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Ula" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Berislav" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Ejiroghene" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Lovre Ivo" });
            participants.Add(new Participant { Id = Guid.NewGuid().ToString().Substring(0, 8), Name = "Michaela" });
            return participants;
        }
        public static void Shuffle<T>(IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}