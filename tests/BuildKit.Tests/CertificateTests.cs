// Copyright (c) Martin Costello, 2025. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Formats.Asn1;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MartinCostello.BuildKit;

public static class CertificateTests
{
    [Fact]
    public static async Task Local_Development_Certificate_Can_Be_Loaded()
    {
        // Arrange
        var metadata = typeof(CertificateTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .ToArray();

        var fileName = metadata.First((p) => p.Key is "CertificateFileName").Value;
        var password = metadata.First((p) => p.Key is "CertificatePassword").Value;

        // Act
        var actual = await File.ReadAllBytesAsync(fileName!, TestContext.Current.CancellationToken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldNotBeEmpty();

        // Act
        var certificate = X509CertificateLoader.LoadPkcs12(actual, password);

        // Assert
        certificate.ShouldNotBeNull();
        certificate.HasPrivateKey.ShouldBeTrue();

        var alternativeNames = GetSubjectAlternativeNames(certificate);

        alternativeNames.ShouldNotBeNull();
        alternativeNames.ShouldNotBeEmpty();
        alternativeNames.ShouldContain("localhost");
        alternativeNames.ShouldContain("127.0.0.1");
    }

    private static List<string> GetSubjectAlternativeNames(X509Certificate2 certificate)
    {
        var oid = new Oid("2.5.29.17"); // Subject Alternative Name OID

        var result = new List<string>();

        foreach (var extension in certificate.Extensions.Where((p) => p.Oid?.Value == oid.Value))
        {
            var reader = new AsnReader(extension.RawData, AsnEncodingRules.DER);
            var sequence = reader.ReadSequence();

            while (sequence.HasData)
            {
                var tag = sequence.PeekTag();

                if (tag.TagClass is not TagClass.ContextSpecific || tag is not { TagValue: 2 or 7 } tagValue)
                {
                    sequence.ReadEncodedValue();
                    continue;
                }

                if (tag.TagValue is 2)
                {
                    result.Add(sequence.ReadCharacterString(UniversalTagNumber.IA5String, tag));
                }
                else
                {
                    var address = sequence.ReadEnumeratedBytes(tag);
                    result.Add(new IPAddress(address.Span).ToString());
                }
            }

            return result;
        }

        return result;
    }
}
