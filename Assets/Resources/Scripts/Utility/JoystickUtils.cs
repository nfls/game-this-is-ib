using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public static class JoystickUtil {

	#region SDL2 Bridge

	/*
	 * This part of work is based on the project SDL2# - C# Wrapper for SDL2 by Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
	 * Copyright (c) 2013-2016 Ethan Lee.
	 * https://github.com/flibitijibibo/SDL2-CS
	 */

	public const string NATIVE_LIB_NAME = "SDL2";
	public const uint SDL_INIT_TIMER = 0x00000001;
	public const uint SDL_INIT_AUDIO = 0x00000010;
	public const uint SDL_INIT_VIDEO = 0x00000020;
	public const uint SDL_INIT_JOYSTICK = 0x00000200;
	public const uint SDL_INIT_HAPTIC = 0x00001000;
	public const uint SDL_INIT_GAMECONTROLLER =	0x00002000;
	public const uint SDL_INIT_EVENTS =	0x00004000;
	public const uint SDL_INIT_SENSOR =	0x00008000;
	public const uint SDL_INIT_NOPARACHUTE = 0x00100000;
	public const uint SDL_INIT_EVERYTHING = (
		SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO | 
		SDL_INIT_EVENTS | SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC |
		SDL_INIT_GAMECONTROLLER | SDL_INIT_SENSOR
	);

	// todo customized rumble effects
	
	public const ushort SDL_HAPTIC_CONSTANT = 1 << 0;
	public const ushort SDL_HAPTIC_SINE = 1 << 1;
	public const ushort SDL_HAPTIC_LEFTRIGHT = 1 << 2;
	public const ushort SDL_HAPTIC_TRIANGLE = 1 << 3;
	public const ushort SDL_HAPTIC_SAWTOOTHUP = 1 << 4;
	public const ushort SDL_HAPTIC_SAWTOOTHDOWN = 1 << 5;
	public const ushort SDL_HAPTIC_SPRING = 1 << 7;
	public const ushort SDL_HAPTIC_DAMPER = 1 << 8;
	public const ushort SDL_HAPTIC_INERTIA = 1 << 9;
	public const ushort SDL_HAPTIC_FRICTION = 1 << 10;
	public const ushort SDL_HAPTIC_CUSTOM = 1 << 11;
	public const ushort SDL_HAPTIC_GAIN = 1 << 12;
	public const ushort SDL_HAPTIC_AUTOCENTER = 1 << 13;
	public const ushort SDL_HAPTIC_STATUS = 1 << 14;
	public const ushort SDL_HAPTIC_PAUSE = 1 << 15;

	public const byte SDL_HAPTIC_POLAR = 0;
	public const byte SDL_HAPTIC_CARTESIAN = 1;
	public const byte SDL_HAPTIC_SPHERICAL = 2;

	public const uint SDL_HAPTIC_INFINITY = 4292967295U;
	
	public const int SDL_QUERY = -1;
	public const int SDL_IGNORE = 0;
	public const int SDL_ENABLE = 1;

	// Operation State Int
	public const int SDL_SUCCESS = 0;

	// CPP BOOL
	public static class SDL_BOOL {
		public const int SDL_FALSE = 0;
		public const int SDL_TRUE = 1;
	}
	
	public enum SDL_JoystickPowerLevel
	{
		SDL_JOYSTICK_POWER_UNKNOWN = -1,
		SDL_JOYSTICK_POWER_EMPTY,
		SDL_JOYSTICK_POWER_LOW,
		SDL_JOYSTICK_POWER_MEDIUM,
		SDL_JOYSTICK_POWER_FULL,
		SDL_JOYSTICK_POWER_WIRED,
		SDL_JOYSTICK_POWER_MAX
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_HapticDirection {
		public byte type;
		public fixed int dir[3];
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_HapticConstant {
		public ushort type;
		public SDL_HapticDirection direction;

		public uint length;
		public ushort delay;

		public ushort button;
		public ushort interval;

		public short level;

		public ushort attack_length;
		public ushort attack_level;
		public ushort fade_length;
		public ushort fade_level;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_HapticPeriodic {
		public ushort type;
		public SDL_HapticDirection direction;

		public uint length;
		public ushort delay;

		public ushort button;
		public ushort interval;

		public ushort period;
		public short magnitude;
		public short offset;
		public ushort phase;

		public ushort attack_length;
		public ushort attack_level;
		public ushort fade_length;
		public ushort fade_level;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_HapticCondition {
		public ushort type;
		public SDL_HapticDirection direction;

		public uint length;
		public ushort delay;

		public ushort button;
		public ushort interval;

		public fixed ushort right_sat[3];
		public fixed ushort left_sat[3];
		public fixed short right_coeff[3];
		public fixed short left_coeff[3];
		public fixed ushort deadband[3];
		public fixed short center[3];
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_HapticRamp {
		public ushort type;
		public SDL_HapticDirection direction;

		public uint length;
		public ushort delay;

		public ushort button;
		public ushort interval;

		public short start;
		public short end;

		public ushort attack_length;
		public ushort attack_level;
		public ushort fade_length;
		public ushort fade_level;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_HapticLeftRight {
		public ushort type;

		public uint length;

		public ushort large_magnitude;
		public ushort small_magnitude;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_HapticCustom {
		public ushort type;
		public SDL_HapticDirection direction;

		public uint length;
		public ushort delay;

		public ushort button;
		public ushort interval;

		public byte channels;
		public ushort period;
		public ushort samples;
		public IntPtr data;

		public ushort attack_length;
		public ushort attack_level;
		public ushort fade_length;
		public ushort fade_level;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct SDL_HapticEffect {
		[FieldOffset(0)] public ushort type;
		[FieldOffset(0)] public SDL_HapticConstant constant;
		[FieldOffset(0)] public SDL_HapticPeriodic periodic;
		[FieldOffset(0)] public SDL_HapticCondition condition;
		[FieldOffset(0)] public SDL_HapticRamp ramp;
		[FieldOffset(0)] public SDL_HapticLeftRight leftright;
		[FieldOffset(0)] public SDL_HapticCustom custom;
	}

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_Init(uint flags);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_InitSubSystem(uint flags);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_Quit();
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_QuitSubSystem(uint flags);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern uint SDL_WasInit(uint flags);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_HapticClose(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_HapticDestroyEffect(IntPtr haptic, int effect);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticEffectSupported(IntPtr haptic, ref SDL_HapticEffect effect);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticGetEffectStatus(IntPtr haptic, int effect);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticIndex(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, EntryPoint = "SDL_HapticName", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr INTERNAL_SDL_HapticName(int device_index);

	private static string SDL_HapticName(int device_index) => UTF8_ToManaged(INTERNAL_SDL_HapticName(device_index));

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticNewEffect(IntPtr haptic, ref SDL_HapticEffect effect);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticNumAxes(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticNumEffects(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticNumEffectsPlaying(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SDL_HapticOpen(int device_index);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticOpened(int device_index);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SDL_HapticOpenFromJoystick(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticPause(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern uint SDL_HapticQuery(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticRumbleInit(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticRumblePlay(IntPtr haptic, float strength, uint length);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticRumbleStop(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticRumbleSupported(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticRunEffect(IntPtr haptic, int effect, uint iterations);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticSetAutocenter(IntPtr haptic, int autocenter);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticSetGain(IntPtr haptic, int gain);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticStopAll(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticStopEffect(IntPtr haptic, int effect);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticUnpause(IntPtr haptic);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_HapticUpdateEffect(IntPtr haptic, int effect, ref SDL_HapticEffect data);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	public static extern int SDL_JoystickGetAttached(IntPtr joystick);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	public static extern int SDL_JoystickIsHaptic(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_NumHaptics();
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickRumble(IntPtr joystick, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_JoystickClose(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickEventState(int state);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern short SDL_JoystickGetAxis(IntPtr joystick, int axis);
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickGetBall(IntPtr joystick, int ball, out int dx, out int dy);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern byte SDL_JoystickGetButton(IntPtr joystick, int button);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern byte SDL_JoystickGetHat(IntPtr joystick, int hat);
	
	[DllImport(NATIVE_LIB_NAME, EntryPoint = "SDL_JoystickName", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr INTERNAL_SDL_JoystickName(IntPtr joystick);
	
	private static string SDL_JoystickName(IntPtr joystick) => UTF8_ToManaged(INTERNAL_SDL_JoystickName(joystick));

	[DllImport(NATIVE_LIB_NAME, EntryPoint = "SDL_JoystickNameForIndex", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr INTERNAL_SDL_JoystickNameForIndex(int device_index);
	
	private static string SDL_JoystickNameForIndex(int device_index) => UTF8_ToManaged(INTERNAL_SDL_JoystickNameForIndex(device_index));

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickNumAxes(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickNumBalls(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickNumButtons(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_JoystickNumHats(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SDL_JoystickOpen(int device_index);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_JoystickUpdate();

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern int SDL_NumJoysticks();
	
	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern SDL_JoystickPowerLevel SDL_JoystickCurrentPowerLevel(IntPtr joystick);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SDL_malloc(IntPtr size);

	[DllImport(NATIVE_LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
	private static extern void SDL_free(IntPtr memblock);

	private static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false) {
		if (s == IntPtr.Zero) return null;
		byte* ptr = (byte*) s;
		while (*ptr != 0) ptr++;
		string result = System.Text.Encoding.UTF8.GetString((byte*) s, (int) (ptr - (byte*) s));
		if (freePtr) SDL_free(s);
		return result;
	}

	#endregion

	public static int HapticDeviceNum => SDL_NumHaptics();
	public static int JoystickNum => SDL_NumJoysticks();

	public static string[] HapticDeviceNames {
		get {
			int deviceNum = HapticDeviceNum;
			if (deviceNum < 0) return null;
			string[] deviceNames = new string[deviceNum];
			for (int i = 0; i < deviceNum; i++) deviceNames[i] = SDL_HapticName(i);
			return deviceNames;
		}
	}

	public static string[] JoystickNames => availableJoystickNames.Values.ToArray();

	private static readonly Dictionary<int, IntPtr> availableHapticDevices = new Dictionary<int, IntPtr>();
	private static readonly Dictionary<int, IntPtr> availableJoysticks = new Dictionary<int, IntPtr>();
	private static readonly Dictionary<int, string> availableJoystickNames = new Dictionary<int, string>();

	public static void Init() => SDL_Init(SDL_INIT_VIDEO | SDL_INIT_HAPTIC | SDL_INIT_JOYSTICK);

	public static void Quit() => SDL_Quit();

	public static bool Rumble(int deviceIndex, float strength, int milliseconds) {
		if (milliseconds < 0) return false;
		if (!CheckAndInitHapticDevice(deviceIndex)) return false;
		IntPtr device = availableHapticDevices[deviceIndex];
		return SDL_HapticRumblePlay(device, strength, (uint) milliseconds) == SDL_SUCCESS;
	}

	public static bool Rumble(float strength, int milliseconds) => Rumble(0, strength, milliseconds);

	public static bool Rumble(int milliseconds = 1000) => Rumble(0, 1, milliseconds);

	public static void StopRumble(int deviceIndex) {
		if (availableHapticDevices.ContainsKey(deviceIndex)) return;
		SDL_HapticRumbleStop(availableHapticDevices[deviceIndex]);
	}

	public static void StopRumbleAll() {
		foreach (var pair in availableHapticDevices) SDL_HapticRumbleStop(pair.Value);
	}

	private static bool CheckAndInitHapticDevice(int deviceIndex) {
		if (availableHapticDevices.ContainsKey(deviceIndex)) return true;
		if (deviceIndex >= HapticDeviceNum) return false;
		IntPtr ptr = SDL_HapticOpen(deviceIndex);
		if (ptr == IntPtr.Zero) return false;
		if (SDL_HapticRumbleSupported(ptr) == SDL_BOOL.SDL_FALSE) return false;
		if (SDL_HapticRumbleInit(ptr) != SDL_SUCCESS) return false;
		availableHapticDevices[deviceIndex] = ptr;
		Debug.Log("Register Device [" + deviceIndex + "] " + SDL_HapticName(deviceIndex) + " !");
		return true;
	}

	public static void UpdateJoysticks() => SDL_JoystickUpdate();

	public static void CheckJoysticks() {
		uint deleteFlags = 0;
		uint standard = 1;
		foreach (var joystick in availableJoysticks) {
			int result = SDL_JoystickGetAttached(joystick.Value);
			if (result == SDL_BOOL.SDL_FALSE) {
				SDL_JoystickClose(joystick.Value);
				deleteFlags |= standard << joystick.Key;
			}
		}

		int shift = 0;
		while (shift < 32) {
			if ((deleteFlags & (1 << shift)) != 0) {
				Debug.Log("Joystick [" + shift + "] " + availableJoystickNames[shift] + " Has Lost Connection !");
				availableJoystickNames.Remove(shift);
				availableJoysticks.Remove(shift);
			}
			
			deleteFlags = deleteFlags >> 1;
			shift++;
		}
	}

	public static bool GetAxis(int axisIndex, out float value) => GetAxis(0, axisIndex, out value);

	public static bool GetAxis(int joystickIndex, int axisIndex, out float value) {
		value = 0;
		if (!CheckAndInitJoystick(joystickIndex) || axisIndex >= SDL_JoystickNumAxes(availableJoysticks[joystickIndex])) return false;
		float r = SDL_JoystickGetAxis(availableJoysticks[joystickIndex], axisIndex);
		value = r / short.MaxValue;
		return true;
	}

	public static bool GetButton(int joystickIndex, int buttonIndex, out bool value) {
		value = false;
		if (!CheckAndInitJoystick(joystickIndex) || buttonIndex >= SDL_JoystickNumButtons(availableJoysticks[joystickIndex])) return false;
		value = SDL_JoystickGetButton(availableJoysticks[joystickIndex], buttonIndex) == 1;
		return true;
	}

	public static bool RumbleJoystick(ushort lowFrequencyStrength, ushort highFrequencyStrength, uint milliseconds) => RumbleJoystick(0, lowFrequencyStrength, highFrequencyStrength, milliseconds);
	
	public static bool RumbleJoystick(int joystickIndex, ushort lowFrequencyStrength, ushort highFrequencyStrength, uint milliseconds) {
		if (!CheckAndInitJoystick(joystickIndex)) return false;
		int result = SDL_JoystickRumble(availableJoysticks[joystickIndex], lowFrequencyStrength, highFrequencyStrength, milliseconds);
		if (result == 0 || result == -1) return false;
		return true;
	}

	public static SDL_JoystickPowerLevel GetJoystickPowerLevel() => GetJoystickPowerLevel(0);

	public static SDL_JoystickPowerLevel GetJoystickPowerLevel(int joystickIndex) {
		if (!CheckAndInitJoystick(joystickIndex)) return SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_UNKNOWN;
		return SDL_JoystickCurrentPowerLevel(availableJoysticks[joystickIndex]);
	}

	private static bool CheckAndInitJoystick(int joystickIndex) {
		if (availableJoysticks.ContainsKey(joystickIndex)) return true;
		if (joystickIndex >= JoystickNum) return false;
		IntPtr ptr = SDL_JoystickOpen(joystickIndex);
		if (ptr == IntPtr.Zero) return false;
		availableJoysticks[joystickIndex] = ptr;
		availableJoystickNames[joystickIndex] = SDL_JoystickName(ptr);
		Debug.Log("Register Joystick [" + joystickIndex + "] " + availableJoystickNames[joystickIndex] + " With " + SDL_JoystickNumAxes(ptr) + " Axes And " + SDL_JoystickNumButtons(ptr) + " Buttons !");
		Debug.Log(availableJoystickNames[joystickIndex] + (SDL_JoystickIsHaptic(ptr) == SDL_BOOL.SDL_TRUE ? " Is " : "Is Not") + "Haptic");
		return true;
	}
}