using System;
namespace SinSense.Core.Entities
{
    public class WordRelation
    {
        public Guid Id { get; set; }

        public Word FromWord { get; set; }
        public Guid FromWordId { get; set; }

        public Word ToWord { get; set; }
        public Guid ToWordId { get; set; }

        public RelationType Type { get; set; }
    }

    public enum RelationType
    {
        None = 0 ,
        Dictionary = 1,
        Lemma = 2,
        Stem = 3
    }
}
