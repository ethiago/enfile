using System;
using Xunit;
using Enfile;
using Enfile.ApiIntegration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var sessao = FMClient.Connect("http://localhost:5000");

            FMFile file = FMFile.OpenWrite(sessao, "TesteArquivo2.ctf", DateTime.Now);

            List<Task> list = new List<Task>();
            for(int i = 0; i < 10000; ++i)
            {
                var task = file.WriteBlockAsync(sessao, $"Esse é o block de dados numero {i}\n");
                list.Add(task);
            }

            Task.WaitAll(list.ToArray());


            file.SaveFile(sessao).Wait();
        }

        [Fact]
        public void Test2()
        {
            var sessao = FMClient.Connect("http://localhost:5000");

            FMFile file = FMFile.GetFileInfo(sessao, new Guid("4b04a325-872c-44c1-adcd-567fd543bcb1"));

            file.SaveFile(sessao).Wait();            
        }
    }
}
