Dialogue and Outcome System (Tweaked by Nikko)
________________________________________________________________________________________________________________

Setting Dialogues W/ Choices
- Set numChoice for number of choices.
    > (i.e At element 0 with numChoice of 4, player will see 4 choices at the first set of dialogue)

- Edit [Choices] based on the number of choices

    > [ChoiceStat] - Stat Requirement for that choice. (Else, set it to none)
    > [ChoiceStatReq] - Stat Requirement Value for the chosen Stat.
    > [Sentence] - Sentence accompanying the choice.

    - Edit [Success (Outcome)]
        > Name
        > Sentence

    - Edit [Failed (Outcome)]
        > Name
        > Sentence
________________________________________________________________________________________________________________

Setting Dialogues Based On Trigger (Hardcoded to detect on a naming basis)
- InteractContainer
    - Dialogue[Set To "Trigger" if trigger-based or Interact if interact-based]

Example
- InteractContainer
    - DialogueInteract (For Interaction) 
            or
    - DialogueTrigger (For Trigger)
*(see prefab for reference)

________________________________________________________________________________________________________________

Note:
    > Always set numChoice regardless if there are no choices. 
        Set it to the amount of dialogue line breaks for that dialogue set.
        (See Prefab for reference.)

    > You can leave the [Choice] empty if there are no plans for a choice.

    > Do not forget to set the proper parameters for the [Outcome] per [Choice].
        