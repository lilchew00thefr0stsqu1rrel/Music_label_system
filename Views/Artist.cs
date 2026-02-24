using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace MvcMusicDistr.Models;

//Приличный человек
//[Index(nameof(Nickname), IsUnique = true)]

public class Artist
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    [Display(Name = "Псевдоним, название группы, имя")]

    public string? Nickname { get; set; }
    [StringLength(64)]
    [Display(Name = "Животное - символ")]
    public string? Animal { get; set; }
    [Display(Name = "Химикат, обозначающий животное")]
    public string? Substance { get; set; }

    [Display(Name = "Причина выбора животного")]
    public string? Reason { get; set; }
}
    