// From: https://github.com/johnsheehan/jwt

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;

namespace JWT
{
    public enum JwtHashAlgorithm
    {
        HS256,
        HS384,
        HS512,
        RS256,
    }

    /// <summary>
    /// Provides methods for encoding and decoding JSON Web Tokens.
    /// </summary>
    public static class JsonWebToken
    {
        private static Dictionary<JwtHashAlgorithm, Func<string, byte[], byte[], bool>> HashAlgorithms;
        private static JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<string, byte[], byte[], bool>>
            {
                { JwtHashAlgorithm.HS256, CreateHMACVerifier(key => new HMACSHA256(key)) },
                { JwtHashAlgorithm.HS384, CreateHMACVerifier(key => new HMACSHA384(key)) },
                { JwtHashAlgorithm.HS512, CreateHMACVerifier(key => new HMACSHA512(key)) },
                { JwtHashAlgorithm.RS256, VerifyRSAHash("SHA256") }
            };
        }

        private static Func<string, byte[], byte[], bool> CreateHMACVerifier(Func<byte[], HMAC> hmacFac)
        {
            return (key, value, sig) =>
            {
                using (var hmac = hmacFac(Encoding.UTF8.GetBytes(key)))
                {
                    var computed = hmac.ComputeHash(value);
                    return String.Equals(
                        Convert.ToBase64String(computed),
                        Convert.ToBase64String(sig),
                        StringComparison.Ordinal);
                }
            };
        }

        private static Func<string, byte[], byte[], bool> VerifyRSAHash(string algorithm)
        {
            return (key, value, signature) =>
            {
                // Key is Base64-Encoded X.509 Cert
                var cert = new X509Certificate2(Convert.FromBase64String(key));
                using (var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key)
                {

                    return rsa.VerifyData(value, "SHA256", signature);
                }
            };
        }

        /// <summary>
        /// Creates a JWT given a payload, the signing key, and the algorithm to use.
        /// </summary>
        /// <param name="payload">An arbitrary payload (must be serializable to JSON via <see cref="System.Web.Script.Serialization.JavaScriptSerializer"/>).</param>
        /// <param name="key">The key used to sign the token.</param>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <returns>The generated JWT.</returns>
        //public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        //{
        //    var segments = new List<string>();
        //    var header = new { typ = "JWT", alg = algorithm.ToString() };

        //    byte[] headerBytes = Encoding.UTF8.GetBytes(jsonSerializer.Serialize(header));
        //    byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonSerializer.Serialize(payload));

        //    segments.Add(Base64UrlEncode(headerBytes));
        //    segments.Add(Base64UrlEncode(payloadBytes));

        //    var stringToSign = string.Join(".", segments.ToArray());

        //    var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);
            
        //    byte[] signature = HashAlgorithms[algorithm](key, bytesToSign);
        //    segments.Add(Base64UrlEncode(signature));

        //    return string.Join(".", segments.ToArray());
        //}

        /// <summary>
        /// Given a JWT, decode it and return the JSON payload.
        /// </summary>
        /// <param name="token">The JWT.</param>
        /// <param name="key">The key that was used to sign the JWT.</param>
        /// <param name="verify">Whether to verify the signature (default is true).</param>
        /// <returns>A string containing the JSON payload.</returns>
        /// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
        public static string Decode(string token, string key, bool verify = true)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = jsonSerializer.Deserialize<Dictionary<string, object>>(headerJson);
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));

            if (verify)
            {
                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var algorithm = (string)headerData["alg"];

                if (!HashAlgorithms[GetHashAlgorithm(algorithm)](key, bytesToSign, crypto))
                {
                    throw new SignatureVerificationException(string.Format("Invalid signature"));
                }
            }

            return payloadJson;
        }

        /// <summary>
        /// Given a JWT, decode it and return the payload as an object (by deserializing it with <see cref="System.Web.Script.Serialization.JavaScriptSerializer"/>).
        /// </summary>
        /// <param name="token">The JWT.</param>
        /// <param name="key">The key that was used to sign the JWT.</param>
        /// <param name="verify">Whether to verify the signature (default is true).</param>
        /// <returns>An object representing the payload.</returns>
        /// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
        public static object DecodeToObject(string token, string key, bool verify = true)
        {
            var payloadJson = JsonWebToken.Decode(token, key, verify);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
            return payloadData;
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "HS256": return JwtHashAlgorithm.HS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                case "RS256": return JwtHashAlgorithm.RS256;
                default: throw new SignatureVerificationException("Algorithm not supported.");
            }
        }

        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }

    public class SignatureVerificationException : Exception
    {
        public SignatureVerificationException(string message)
            : base(message)
        {
        }
    }
}