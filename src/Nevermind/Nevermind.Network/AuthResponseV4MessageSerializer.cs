﻿/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

using Nevermind.Core.Crypto;
using Nevermind.Core.Encoding;
using Nevermind.Core.Extensions;

namespace Nevermind.Network
{
    public class AuthResponseV4MessageSerializer : IMessageSerializer<AuthResponseV4Message>
    {
        public const int EphemeralPublicKeyLength = 64;
        public const int EphemeralPublicKeyOffset = 0;
        public const int NonceLength = 32;
        public const int NonceOffset = EphemeralPublicKeyOffset + EphemeralPublicKeyLength;
        public const int VersionOffset = NonceOffset + NonceLength;
        public const int TotalLength = EphemeralPublicKeyLength + NonceLength;

        public byte[] Serialize(AuthResponseV4Message message)
        {
            return Rlp.Encode(
                Rlp.Encode(message.EphemeralPublicKey.PrefixedBytes.Slice(1, 64)),
                Rlp.Encode(message.Nonce),
                Rlp.Encode(message.Version)
            ).Bytes;
        }

        public AuthResponseV4Message Deserialize(byte[] bytes)
        {
            Rlp rlp = new Rlp(bytes);
            object[] decodedRaw = (object[])Rlp.Decode(rlp);

            AuthResponseV4Message authV4Message = new AuthResponseV4Message();
            authV4Message.EphemeralPublicKey = new PublicKey((byte[])decodedRaw[0]);
            authV4Message.Nonce = (byte[])decodedRaw[1];
            // TODO: check the version? /Postel
            return authV4Message;
        }
    }
}