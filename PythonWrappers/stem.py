# Imports
from nltk.stem import PorterStemmer
import sys

# get the input sentence
sentence = sys.argv[1]

# If input is empty exit with code 1
if(not sentence or len(sentence) < 1 )
{
    exit(1);
}

# Create the lemmatizer instance
eng_stemmer = PorterStemmer()    # English Stemmer

# get the lemmas of the tokens
tokens = sentence.split(" ")
stems = []
for token in tokens:
    stems.append(eng_stemmer.stem(token))

# print the output in a space seperated string
print(" ".join(stems));