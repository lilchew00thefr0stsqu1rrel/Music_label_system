using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMusicDistr.Models;

public class ArtistSubstanceViewModel
{
    public List<Artist>? Artists { get; set; }
    public SelectList? Substances { get; set; }
    public string? ArtistSubstance { get; set; }
    public string? SearchString { get; set; }
}