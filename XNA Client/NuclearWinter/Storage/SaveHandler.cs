using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Storage;

namespace NuclearWinter.Storage
{
    public abstract class SaveHandler
    {
        protected NuclearGame                                   Game;

        public string                                           FileName                = "SaveData.bin";

        public readonly UInt32                                  SettingsMagicNumber     = 0xffffffff;
        protected Dictionary<UInt32,Action<BinaryReader>>       ReadSettingsActions;

#if WINDOWS || LINUX || MACOSX || XBOX
        public readonly UInt32                                  DataMagicNumber         = 0xffffffff;
        protected Dictionary<UInt32,Action<BinaryReader>>       ReadDataActions;

        protected string                                        ContainerName;
#endif

        //----------------------------------------------------------------------
        protected abstract void WriteSettings( BinaryWriter _output );
#if WINDOWS || LINUX || MACOSX || XBOX
        protected abstract void WriteData( BinaryWriter _output );
#endif

        //----------------------------------------------------------------------
#if WINDOWS_PHONE
        public SaveData( NuclearGame _game, UInt32 _uiSettingsMagicNumber )
        {
#else
        public SaveHandler( NuclearGame _game, UInt32 _uiSettingsMagicNumber, string _strContainerName, UInt32 _uiDataMagicNumber )
        {
            Debug.Assert( _uiDataMagicNumber != 0xffffffff );
            DataMagicNumber         = _uiDataMagicNumber;

            ContainerName   = _strContainerName;
#endif
            Debug.Assert( _uiSettingsMagicNumber != 0xffffffff );
            SettingsMagicNumber     = _uiSettingsMagicNumber;

            Game            = _game;
        }

        //----------------------------------------------------------------------
        // Save settings
        public void SaveGameSettings()
        {
            try
            {
#if WINDOWS || LINUX || MACOSX
                using( var store = IsolatedStorageFile.GetUserStoreForDomain() )
#else
                using( var store = IsolatedStorageFile.GetUserStoreForApplication() )
#endif
                {
                    try {
                        var stream = store.OpenFile( FileName, FileMode.Create );

                        if( stream != null )
                        {
                            var output = new BinaryWriter( stream );
                            output.Write( SettingsMagicNumber );
                            WriteSettings( output );

                            stream.Close();
                        }
                    }
                    catch
                    {
                    }

                    store.Dispose();
                }
            }
            catch
            {
                Debug.Assert( false, "Saving settings has failed" );
            }
        }

        protected abstract void ResetSettings();

        //----------------------------------------------------------------------
        // Load game settings
        public void LoadGameSettings()
        {
            ResetSettings();

            try
            {
#if WINDOWS || LINUX || MACOSX
                using( var store = IsolatedStorageFile.GetUserStoreForDomain() )
#else
                using( var store = IsolatedStorageFile.GetUserStoreForApplication() )
#endif
                {
                    if( store.FileExists( FileName ) )
                    {
                        try
                        {
                            var stream = store.OpenFile( FileName, FileMode.Open );

                            if( stream != null )
                            {
                                using ( var input = new BinaryReader( stream ) )
                                {
                                    uint magicNumber = 0xffffffff;

                                    try
                                    {
                                        magicNumber = input.ReadUInt32();

                                        if( ReadSettingsActions.ContainsKey( magicNumber ) )
                                        {
                                            ReadSettingsActions[ magicNumber ]( input );
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }

                                stream.Close();
                            }
                        }
                        catch
                        {
                        }

                        store.Dispose();
                    }
                }
            }
            catch
            {
                Debug.Assert( false, "Loading settings has failed" );
            }
        }

#if WINDOWS || LINUX || MACOSX || XBOX
        //----------------------------------------------------------------------
        // Save game data
        public void SaveGameData()
        {
            try
            {
                if( Game.SaveGameStorageDevice != null && Game.SaveGameStorageDevice.IsConnected )
                {
                    IAsyncResult result = Game.SaveGameStorageDevice.BeginOpenContainer( ContainerName, null, null );
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = Game.SaveGameStorageDevice.EndOpenContainer( result );
                    result.AsyncWaitHandle.Close();

                    try {
                        var stream = container.OpenFile( FileName, FileMode.Create );

                        if( stream != null )
                        {
                            var output = new BinaryWriter( stream );
                            output.Write( DataMagicNumber );
                            WriteData( output );

                            stream.Close();
                        }
                    }
                    catch
                    {
                    }

                    container.Dispose();
                }
            }
            catch
            {
                Debug.Assert( false, "Saving save data has failed" );
            }
        }

        protected abstract void ResetData();

        //---------------------------------------------------------------------
        // Load game data
        public void LoadGameData()
        {
            ResetData();

            try
            {
                if( Game.SaveGameStorageDevice != null && Game.SaveGameStorageDevice.IsConnected )
                {
                    IAsyncResult result = Game.SaveGameStorageDevice.BeginOpenContainer( ContainerName, null, null );
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = Game.SaveGameStorageDevice.EndOpenContainer( result );
                    result.AsyncWaitHandle.Close();

                    if( container.FileExists( FileName ) )
                    {
                        try
                        {
                            var stream = container.OpenFile( FileName, FileMode.Open );

                            if( stream != null )
                            {
                                using ( var input = new BinaryReader( stream ) )
                                {
                                    uint magicNumber = 0xffffffff;

                                    try
                                    {
                                        magicNumber = input.ReadUInt32();

                                        if( ReadDataActions.ContainsKey( magicNumber ) )
                                        {
                                            ReadDataActions[ magicNumber ]( input );
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }

                                stream.Close();
                            }
                        }
                        catch
                        {
                        }

                        container.Dispose();
                    }
                }
            }
            catch
            {
                Debug.Assert( false, "Loading save data has failed" );
            }
        }
#endif
    }
}
