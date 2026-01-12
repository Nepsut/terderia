using UnityEngine;

namespace CardSystem
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private TextAsset cardTsvAsset;
        [SerializeField] private string[] tableHeaders;
        public Card[] Cards { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        private void AssignCardDataFromFile()
        {
            string[] cardDataAsStrings = cardTsvAsset.text.Split('\n');
            int arrayLength = cardDataAsStrings.Length;

            for (int i = 1; i < arrayLength; i++)
            {
                
            }
        }
    }
}