using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public List<AudioClip> eatSounds;
	public List<AudioClip> wrongSounds;
	public List<AudioClip> changeSounds;

	AudioSource a;
	public static Sounds s;

	static List<AudioClip> seatSounds;
	static List<AudioClip> swrongSounds;
	static List<AudioClip> schangeSounds;

	public AudioClip pourSound;
	public AudioClip blenderSound;
	public AudioClip snapSound;

	static AudioClip spourSound;
	static AudioClip sBlenderSound;
	static AudioClip ssnapSound;

	void Start() {
		a = GetComponent<AudioSource>();
		s = this;

		seatSounds = eatSounds;
		swrongSounds = wrongSounds;
		schangeSounds = changeSounds;

		spourSound = pourSound;
		sBlenderSound = blenderSound;
		ssnapSound = snapSound;
	}

	public static void EatSound() {
		s.a.PlayOneShot(getRandomSound(seatSounds));
	}

	public static void WrongSound() {
		s.a.PlayOneShot(getRandomSound(swrongSounds));
	}

	public static void ChangeSound() {
		s.a.PlayOneShot(getRandomSound(schangeSounds));
	}

	public static void PourSound() {
		s.a.PlayOneShot(spourSound);
	}

	public static void BlendSound() {
		s.a.PlayOneShot(sBlenderSound);
	}

	public static void SnapSound() {
		s.a.PlayOneShot(ssnapSound);
	}

	public static AudioClip getRandomSound(List<AudioClip> sounds) {
		return sounds[Random.Range(0, sounds.Count)];
	}

}
