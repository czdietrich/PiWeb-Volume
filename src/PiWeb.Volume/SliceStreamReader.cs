﻿#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.IMT.PiWeb.Volume
{
    #region usings

    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Zeiss.IMT.PiWeb.Volume.Interop;

    #endregion

    internal class SliceStreamReader
    {
        #region members

        private readonly IProgress<VolumeSliceDefinition> _ProgressNotifier;
        private readonly CancellationToken _Ct;

        private readonly ushort _SizeX;
        private readonly ushort _SizeY;
        private readonly ushort _SizeZ;
        private ushort _CurrentSlice;
        private byte[] _Buffer;
        private Stream _Stream;

        #endregion

        #region constructors

        internal SliceStreamReader( VolumeMetadata metadata, Stream stream, IProgress<VolumeSliceDefinition> progressNotifier = null, CancellationToken ct = default( CancellationToken ) )
        {
            _Stream = stream;

            _ProgressNotifier = progressNotifier;
            _Ct = ct;
            _CurrentSlice = 0;

            _SizeX = metadata.SizeX;
            _SizeY = metadata.SizeY;
            _SizeZ = metadata.SizeZ;

            _Buffer = new byte[0];

            Interop = new InteropSliceReader
            {
                ReadSlice = ReadSlice
            };
        }

        #endregion

        #region properties

        internal InteropSliceReader Interop { get; }

        #endregion

        #region methods

        internal bool ReadSlice( IntPtr pv, ushort width, ushort height )
        {
            _Ct.ThrowIfCancellationRequested();

            if( _CurrentSlice >= _SizeZ )
                return false;

            _ProgressNotifier?.Report( new VolumeSliceDefinition( Direction.Z, _CurrentSlice ) );

            if( _Buffer.Length < width * height )
                _Buffer = new byte[width * height];

            for( var y = 0; y < _SizeY; y++ )
                _Stream.Read( _Buffer, y * width, _SizeX );


            Marshal.Copy( _Buffer, 0, pv, width * height );
            _CurrentSlice++;

            return true;
        }

        #endregion
    }
}