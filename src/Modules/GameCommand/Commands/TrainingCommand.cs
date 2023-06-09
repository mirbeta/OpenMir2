﻿using SystemModule;

namespace CommandSystem {
    [Command("Training", "", 10)]
    public class TrainingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            if (PlayerActor.Permission < 6) {
                return;
            }
        }
    }
}