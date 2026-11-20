using System;
using System.Collections.Generic;
class Song
{
    string name = "";
    string author = "";
    Song? prev;
    public Song()
    {
        name = "";
        author = "";
        prev = null;
    }
    public Song(string name, string author)
    {
        this.name = name;
        this.author = author;
        prev = null;
    }
    public Song(string name, string author, Song? prev)
    {
        this.name = name;
        this.author = author;
        this.prev = prev;
    }
    public void Print()
    {
        Console.WriteLine("Название: " + name);
        Console.WriteLine("Автор: " + author);
    }
    public string Title()
    {
        return name + " - " + author;
    }
    public override bool Equals(object? d)
    {
        if (d is Song song)
        {
            return Title() == song.Title();
        }
        return false;
    }
    public override int GetHashCode()
    {
        return Title().GetHashCode();
    }
}
class Program
{
    static void Main()
    {
        List<Song> songs = new List<Song>();
        Song mySong = new Song();
        Song song1 = new Song("Династия", "VILLIAN, madk1d");
        Song song2 = new Song("Экспонат", "MIA BOYKA", song1);
        Song song3 = new Song("Кинолента", "Баста", song2);
        Song song4 = new Song("Завидуют", "Платина", song3);
        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song3);
        songs.Add(song4);
        foreach (Song song in songs)
        {
            song.Print();
            Console.WriteLine();
        }
        if (songs[0].Equals(songs[1]))
        {
            Console.WriteLine("Песни одинаковые.");
        }
        else
        {
            Console.WriteLine("Песни разные.");
        }
    }
}