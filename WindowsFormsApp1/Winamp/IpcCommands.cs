﻿
namespace baxp.Winamp
{
    public enum PlayingStatus
    {
        // Unknown = -1,
        Playing = 1,
        Paused = 3,
        Stopped = 0,
    }

    internal enum Buttons
    {
        Button1 = 40044,
        Button2 = 40045,
        Button3 = 40046,
        Button4 = 40047,
        Button5 = 40048,

        VolumeUp = 40058,
        VolumeDown = 40059,
        FastForward_5Sec = 40060,
        Rewind_5sec = 40061,

        PrevTrack = Button1,
        Play = Button2,
        Pause = Button3,
        Stop = Button4,
        NextTrack = Button5

    }

    internal enum IpcCommands
    {
        IsPlaying = 104,
        SetVolume = 122,
        GetListLength = 124,
        GetListPosition = 125,
        GetNextListPosition = 136,
        SetListPosition = 121,
        IsFullStop = 400,
        IsShuffled = 250,
        ToggleShuffle = 252,
        IsRepeating = 251,
        ToggleRepeating = 253,
        GetPlaylistTitleUnicode = 213,
        GetPlayingTitleUnicode = 3034,
        TogglePlaying = 102,
        NextItem = 636,
        RestartWinamp = 135,

        // IPC_GETNEXTLISTPOS 136

        /*
         
         #define WINAMP_OPTIONS_EQ               40036 // toggles the EQ window
#define WINAMP_OPTIONS_PLEDIT           40040 // toggles the playlist window
#define WINAMP_VOLUMEUP                 40058 // turns the volume up a little
#define WINAMP_VOLUMEDOWN               40059 // turns the volume down a little
#define WINAMP_FFWD5S                   40060 // fast forwards 5 seconds
#define WINAMP_REW5S                    40061 // rewinds 5 seconds


            SendMessage(hwnd_winamp, WM_COMMAND,command_name,0);
// the following are the five main control buttons, with optionally shift 
// or control pressed
// (for the exact functions of each, just try it out)
#define WINAMP_BUTTON1                  40044
#define WINAMP_BUTTON2                  40045
#define WINAMP_BUTTON3                  40046
#define WINAMP_BUTTON4                  40047
#define WINAMP_BUTTON5                  40048
#define WINAMP_BUTTON1_SHIFT            40144
#define WINAMP_BUTTON2_SHIFT            40145
#define WINAMP_BUTTON3_SHIFT            40146
#define WINAMP_BUTTON4_SHIFT            40147
#define WINAMP_BUTTON5_SHIFT            40148
#define WINAMP_BUTTON1_CTRL             40154
#define WINAMP_BUTTON2_CTRL             40155
#define WINAMP_BUTTON3_CTRL             40156
#define WINAMP_BUTTON4_CTRL             40157
#define WINAMP_BUTTON5_CTRL             40158

#define WINAMP_FILE_PLAY                40029 // pops up the load file(s) box
#define WINAMP_FILE_DIR                 40187 // pops up the load directory box
#define WINAMP_OPTIONS_PREFS            40012 // pops up the preferences
#define WINAMP_OPTIONS_AOT              40019 // toggles always on top
#define WINAMP_HELP_ABOUT               40041 // pops up the about box :)
         

        #define IPC_STARTPLAY 102   
#define IPC_STARTPLAY_INT 1102 
/* SendMessage(hwnd_winamp,WM_WA_IPC,0,IPC_STARTPLAY);
** Sending this will start playback and is almost the same as hitting the play button.
** The IPC_STARTPLAY_INT version is used internally and you should not need to use it
** since it won't be any fun.
*/


    }
}
