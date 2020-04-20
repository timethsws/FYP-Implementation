# Imports
from nltk.stem import WordNetLemmatizer
import sys

# get the input sentence
sentence = sys.argv[1]

# If input is empty exit with code 1
if(not sentence or len(sentence) < 1 )
{
    exit(1);
}

# Create the lemmatizer instance
eng_lemmatizer = WordNetLemmatizer()    # English Lemmatizer

# get the lemmas of the tokens
tokens = sentence.split(" ")
lemmas = []
for token in tokens:
    lemmas.append(eng_lemmatizer.lemmatize(token))

# print the output in a space seperated string
print(" ".join(lemmas));
