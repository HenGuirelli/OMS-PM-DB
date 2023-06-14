using Common;
using PM.Collections;
using PM.Configs;

namespace OMS.Repositories.Accounts
{
    public class PmAccountsRepository
    {
        private readonly PmList<Account> _accounts 
            = new(Path.Combine(PmGlobalConfiguration.PmInternalsFolder, "Accounts"));
        private readonly Dictionary<string, Account> _accountsByAccount = new();

        public PmAccountsRepository()
        {
            if (_accounts.Count == 0)
            {
                _accounts.AddPersistent(new Account { AccountNumber = "8834", Name = "Luigi Fogaça" });
                _accounts.AddPersistent(new Account { AccountNumber = "1683", Name = "Luiz Otávio Pereira" });
                _accounts.AddPersistent(new Account { AccountNumber = "2533", Name = "Miguel Costa" });
                _accounts.AddPersistent(new Account { AccountNumber = "9605", Name = "Benjamin Lopes" });
                _accounts.AddPersistent(new Account { AccountNumber = "4415", Name = "Maria Sophia Costa" });
                _accounts.AddPersistent(new Account { AccountNumber = "2738", Name = "Julia Silva" });
                _accounts.AddPersistent(new Account { AccountNumber = "1376", Name = "Samuel Alves" });
                _accounts.AddPersistent(new Account { AccountNumber = "9013", Name = "Stephany Jesus" });
                _accounts.AddPersistent(new Account { AccountNumber = "3832", Name = "Sr. Gabriel Peixoto" });
                _accounts.AddPersistent(new Account { AccountNumber = "4692", Name = "Sr. Danilo Pires" });
                _accounts.AddPersistent(new Account { AccountNumber = "8983", Name = "Isabella Barbosa" });
                _accounts.AddPersistent(new Account { AccountNumber = "7798", Name = "Fernando Freitas" });
                _accounts.AddPersistent(new Account { AccountNumber = "7002", Name = "Thiago Pereira" });
                _accounts.AddPersistent(new Account { AccountNumber = "4611", Name = "Melissa Cavalcanti" });
                _accounts.AddPersistent(new Account { AccountNumber = "5626", Name = "Danilo Jesus" });
                _accounts.AddPersistent(new Account { AccountNumber = "5850", Name = "Sr. Ryan Cavalcanti" });
                _accounts.AddPersistent(new Account { AccountNumber = "1589", Name = "Marina Pereira" });
                _accounts.AddPersistent(new Account { AccountNumber = "8400", Name = "Luiz Henrique Nascimento" });
                _accounts.AddPersistent(new Account { AccountNumber = "8964", Name = "Lorena Moreira" });
                _accounts.AddPersistent(new Account { AccountNumber = "2168", Name = "Bruno da Costa" });
                _accounts.AddPersistent(new Account { AccountNumber = "2816", Name = "Gabrielly da Rosa" });
                _accounts.AddPersistent(new Account { AccountNumber = "1619", Name = "Letícia Peixoto" });
                _accounts.AddPersistent(new Account { AccountNumber = "7552", Name = "Alícia Silva" });
                _accounts.AddPersistent(new Account { AccountNumber = "5857", Name = "Henrique Caldeira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3066", Name = "Sra. Mariana Cardoso" });
                _accounts.AddPersistent(new Account { AccountNumber = "8343", Name = "Luna Azevedo" });
                _accounts.AddPersistent(new Account { AccountNumber = "2731", Name = "João Felipe Fogaça" });
                _accounts.AddPersistent(new Account { AccountNumber = "5208", Name = "Fernanda Moura" });
                _accounts.AddPersistent(new Account { AccountNumber = "9427", Name = "Isadora Costela" });
                _accounts.AddPersistent(new Account { AccountNumber = "6067", Name = "Esther Moreira" });
                _accounts.AddPersistent(new Account { AccountNumber = "8660", Name = "Elisa Nascimento" });
                _accounts.AddPersistent(new Account { AccountNumber = "7057", Name = "Vicente Santos" });
                _accounts.AddPersistent(new Account { AccountNumber = "8091", Name = "Larissa Oliveira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3004", Name = "Dr. Bernardo Nunes" });
                _accounts.AddPersistent(new Account { AccountNumber = "2119", Name = "Enzo Gabriel Ferreira" });
                _accounts.AddPersistent(new Account { AccountNumber = "2667", Name = "Dr. Gustavo Henrique Farias" });
                _accounts.AddPersistent(new Account { AccountNumber = "5831", Name = "Mirella Novaes" });
                _accounts.AddPersistent(new Account { AccountNumber = "1548", Name = "Esther Ribeiro" });
                _accounts.AddPersistent(new Account { AccountNumber = "5365", Name = "Vinicius da Rosa" });
                _accounts.AddPersistent(new Account { AccountNumber = "3052", Name = "Sra. Ana Vitória da Luz" });
                _accounts.AddPersistent(new Account { AccountNumber = "6632", Name = "Luna Monteiro" });
                _accounts.AddPersistent(new Account { AccountNumber = "7469", Name = "Kamilly da Conceição" });
                _accounts.AddPersistent(new Account { AccountNumber = "4420", Name = "Maria Vitória Santos" });
                _accounts.AddPersistent(new Account { AccountNumber = "9805", Name = "Sarah Nunes" });
                _accounts.AddPersistent(new Account { AccountNumber = "3110", Name = "Maria Eduarda Nogueira" });
                _accounts.AddPersistent(new Account { AccountNumber = "4431", Name = "Davi Luiz Carvalho" });
                _accounts.AddPersistent(new Account { AccountNumber = "8285", Name = "Pietro Castro" });
                _accounts.AddPersistent(new Account { AccountNumber = "1817", Name = "Lívia Freitas" });
                _accounts.AddPersistent(new Account { AccountNumber = "3889", Name = "Augusto Melo" });
                _accounts.AddPersistent(new Account { AccountNumber = "5376", Name = "Marcelo da Rosa" });
                _accounts.AddPersistent(new Account { AccountNumber = "1442", Name = "Diego Moura" });
                _accounts.AddPersistent(new Account { AccountNumber = "7998", Name = "Isis Caldeira" });
                _accounts.AddPersistent(new Account { AccountNumber = "2201", Name = "Otávio Lima" });
                _accounts.AddPersistent(new Account { AccountNumber = "7824", Name = "Srta. Ana Júlia da Mota" });
                _accounts.AddPersistent(new Account { AccountNumber = "2648", Name = "Benício Lima" });
                _accounts.AddPersistent(new Account { AccountNumber = "4294", Name = "Lara da Paz" });
                _accounts.AddPersistent(new Account { AccountNumber = "5054", Name = "Bianca Lima" });
                _accounts.AddPersistent(new Account { AccountNumber = "1026", Name = "Dr. Nicolas Rodrigues" });
                _accounts.AddPersistent(new Account { AccountNumber = "2473", Name = "Ana Laura Melo" });
                _accounts.AddPersistent(new Account { AccountNumber = "3129", Name = "Renan Caldeira" });
                _accounts.AddPersistent(new Account { AccountNumber = "4116", Name = "Bernardo Farias" });
                _accounts.AddPersistent(new Account { AccountNumber = "1530", Name = "Murilo Melo" });
                _accounts.AddPersistent(new Account { AccountNumber = "2828", Name = "Ana Luiza da Luz" });
                _accounts.AddPersistent(new Account { AccountNumber = "1881", Name = "Dr. Theo Ribeiro" });
                _accounts.AddPersistent(new Account { AccountNumber = "9146", Name = "André Silveira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3031", Name = "Maria Monteiro" });
                _accounts.AddPersistent(new Account { AccountNumber = "5497", Name = "Vitor Hugo Ribeiro" });
                _accounts.AddPersistent(new Account { AccountNumber = "5521", Name = "Maria Sophia Carvalho" });
                _accounts.AddPersistent(new Account { AccountNumber = "8638", Name = "Enzo Gabriel Araújo" });
                _accounts.AddPersistent(new Account { AccountNumber = "8741", Name = "Olivia Cavalcanti" });
                _accounts.AddPersistent(new Account { AccountNumber = "8461", Name = "Henrique Santos" });
                _accounts.AddPersistent(new Account { AccountNumber = "8797", Name = "Ryan Cardoso" });
                _accounts.AddPersistent(new Account { AccountNumber = "7183", Name = "Luiz Miguel Cardoso" });
                _accounts.AddPersistent(new Account { AccountNumber = "6259", Name = "Raquel Aragão" });
                _accounts.AddPersistent(new Account { AccountNumber = "6362", Name = "Benjamin Gomes" });
                _accounts.AddPersistent(new Account { AccountNumber = "9003", Name = "Nina Rezende" });
                _accounts.AddPersistent(new Account { AccountNumber = "7405", Name = "Dr. Enzo Gabriel Castro" });
                _accounts.AddPersistent(new Account { AccountNumber = "1874", Name = "Henrique Aragão" });
                _accounts.AddPersistent(new Account { AccountNumber = "7872", Name = "Guilherme Cardoso" });
                _accounts.AddPersistent(new Account { AccountNumber = "1940", Name = "Natália Nogueira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3335", Name = "Marcos Vinicius Fogaça" });
                _accounts.AddPersistent(new Account { AccountNumber = "5745", Name = "Yago da Luz" });
                _accounts.AddPersistent(new Account { AccountNumber = "8715", Name = "Dr. Daniel da Cunha" });
                _accounts.AddPersistent(new Account { AccountNumber = "8448", Name = "Srta. Ana Júlia Pereira" });
                _accounts.AddPersistent(new Account { AccountNumber = "4422", Name = "Paulo Barros" });
                _accounts.AddPersistent(new Account { AccountNumber = "2846", Name = "João Guilherme Oliveira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3036", Name = "Dr. Luiz Otávio Aragão" });
                _accounts.AddPersistent(new Account { AccountNumber = "5549", Name = "Samuel Souza" });
                _accounts.AddPersistent(new Account { AccountNumber = "1889", Name = "Ana das Neves" });
                _accounts.AddPersistent(new Account { AccountNumber = "5171", Name = "Lorena Sales" });
                _accounts.AddPersistent(new Account { AccountNumber = "3564", Name = "Sr. Davi Luiz Rocha" });
                _accounts.AddPersistent(new Account { AccountNumber = "6440", Name = "Maysa Sales" });
                _accounts.AddPersistent(new Account { AccountNumber = "3824", Name = "André Moreira" });
                _accounts.AddPersistent(new Account { AccountNumber = "3741", Name = "Beatriz Porto" });
                _accounts.AddPersistent(new Account { AccountNumber = "1421", Name = "Ian Mendes" });
                _accounts.AddPersistent(new Account { AccountNumber = "6124", Name = "Valentina Santos" });
                _accounts.AddPersistent(new Account { AccountNumber = "9989", Name = "Mariane da Conceição" });
                _accounts.AddPersistent(new Account { AccountNumber = "5438", Name = "Vitória Jesus" });
                _accounts.AddPersistent(new Account { AccountNumber = "3925", Name = "Otávio da Luz" });
                _accounts.AddPersistent(new Account { AccountNumber = "2396", Name = "Igor da Cruz" });

                foreach (var account in _accounts)
                {
                    account.ContaCorrente = new ContaCorrente { Value = 1_000_000m };
                    _accountsByAccount[account.AccountNumber] = account;
                }
            }
        }

        public Account GetAccount(string accountNumber)
        {
            return _accountsByAccount[accountNumber];
        }
    }
}
