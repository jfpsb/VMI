using System.Security.Cryptography;
using System.Text;

namespace VandaModaIntimaWpf.ViewModel.Util
{
    public class Autenticacao
    {
        /// <summary>
        /// Confirma se senha informada é igual a senha salva.
        /// </summary>
        /// <param name="senha">Senha informada por usuário</param>
        /// <param name="senhaSalva">Senha do funcionário salva no banco de dados</param>
        /// <returns></returns>
        public static bool ConfirmaSenha(string senha, string senhaSalva)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(senha));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5.Hash.Length; i++)
            {
                sb.Append(md5.Hash[i].ToString("X2"));
            }

            var emString = sb.ToString();

            return emString.Equals(senhaSalva.ToUpper());
        }

        public static string EncriptaEmMD5(string senha)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(senha));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5.Hash.Length; i++)
            {
                sb.Append(md5.Hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
