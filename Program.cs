using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertStoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (StoreLocation storeLocation in (StoreLocation[])Enum.GetValues(typeof(StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])Enum.GetValues(typeof(StoreName)))
                {
                    X509Store store = new X509Store(storeName, storeLocation);
                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);
                        if (store.Certificates.Count > 0)
                        {
                            Console.WriteLine("Store:   {0}, {1}", store.Name, store.Location);
                            Console.WriteLine("=============================================");

                            foreach (X509Certificate2 Cert in store.Certificates)
                            {
                                if (Cert.NotAfter <= DateTime.Now)
                                {
                                    Console.WriteLine("Expired: " + (Cert.FriendlyName != "" ? Cert.FriendlyName + ", " : Cert.Subject) + " (" + Cert.GetExpirationDateString() + ")");
                                }
                                else
                                {
                                    Console.WriteLine("OK:      " + (Cert.FriendlyName != "" ? Cert.FriendlyName + ", " : Cert.Subject) + " (" + Cert.GetExpirationDateString() + ")");
                                }
                            }
                            Console.WriteLine();
                        }
                        store.Close();
                    }
                    catch (CryptographicException)
                    {
                        //store does not exist or access denied
                    }
                }
            }
        }
    }
}
