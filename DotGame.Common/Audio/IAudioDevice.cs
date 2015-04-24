using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt das AudioDevice dar.
    /// </summary>
    public interface IAudioDevice : IDisposable, IEquatable<IAudioDevice>
    {
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Ruft die AudioCapabilities ab, welche Informationen über unterstützte Funktionen geben.
        /// </summary>
        AudioCapabilities Capabilities { get; }

        /// <summary>
        /// Ruft die IAudioFactory ab, die zur Erzeugung neuer AudioObjekte benutzt wird.
        /// </summary>
        IAudioFactory Factory { get; }

        /// <summary>
        /// Ruft den Namen des aktuell benutzten Geräts ab.
        /// </summary>
        string DeviceName { get; }
        
        /// <summary>
        /// Ruft den Namen des Herstellers des Treibers ab.
        /// </summary>
        string VendorName { get; }

        /// <summary>
        /// Ruft den Renderer des Herstellers ab.
        /// </summary>
        string Renderer { get; }
        
        /// <summary>
        /// Ruft die Treiberversion ab.
        /// </summary>
        string DriverVersion { get; }

        /// <summary>
        /// Ruft die API-Version ab.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Ruft die Version der Efx Erweiterung ab.
        /// </summary>
        Version EfxVersion { get; }

        /// <summary>
        /// Ruft eine Liste an allen unterstützten Erweiterungen ab.
        /// </summary>
        ReadOnlyCollection<string> Extensions { get; }

        /// <summary>
        /// Ruft die maximale Anzahl an Routes per Channel ab.
        /// </summary>
        int MaxRoutes { get; }

        /// <summary>
        /// Ruft die IAudioListener-Instanz ab, die den Beobachter darstellt.
        /// </summary>
        IAudioListener Listener { get; }
    }
}
