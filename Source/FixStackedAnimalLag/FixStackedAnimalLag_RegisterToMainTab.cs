using System;
using System.Collections.Generic;
using ModButtons;
using Verse;

namespace FixStackedAnimalLag;

internal class FixStackedAnimalLag_RegisterToMainTab
{
    public static bool wasRegistered;

    public static void ensureMainTabRegistered()
    {
        if (wasRegistered)
        {
            return;
        }

        Log.Message("Hello from FixStackedAnimalLag_RegisterToMainTab ensureMainTabRegistered");

        var columns = MainTabWindow_ModButtons.columns;

        var buttons = new List<ModButton_Text>
        {
            new ModButton_Text(
                delegate
                {
                    var buttonLabel = $"Enforcing Enemy Collision Currently:{Environment.NewLine}";
                    if (FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies)
                    {
                        buttonLabel += "ENABLED";
                    }
                    else
                    {
                        buttonLabel += "DISABLED";
                    }

                    return buttonLabel;
                },
                delegate
                {
                    FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies =
                        !FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies;
                }
            )
        };

        columns.Add(buttons);

        wasRegistered = true;
    }
}