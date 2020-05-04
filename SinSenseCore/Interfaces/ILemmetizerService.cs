using System;
using System.Collections.Generic;
using SinSense.Core.Entities;

namespace SinSense.Core.Interfaces
{
    public interface ILemmetizerService
    {
        Language Language { get;}

        List<string> Lemmetize(string text);

        string LemmetizeWord(string word);
    }
}
