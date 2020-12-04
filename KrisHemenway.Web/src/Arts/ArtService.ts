import { Observable } from "@residualeffect/reactor";
import {default as PunchOutBaldBull} from "Arts/Images/PunchOut-BaldBull.png";
import {default as PunchOutDonFlamenco} from "Arts/Images/PunchOut-DonFlamenco.png";
import {default as PunchOutGlassJoe} from "Arts/Images/PunchOut-GlassJoe.png";
import {default as PunchOutGreatTiger} from "Arts/Images/PunchOut-GreatTiger.png";
import {default as PunchOutKingHippo} from "Arts/Images/PunchOut-KingHippo.png";
import {default as PunchOutPistonHonda} from "Arts/Images/PunchOut-PistonHonda.png";
import {default as PunchOutSodaPopinski} from "Arts/Images/PunchOut-SodaPopinski.png";
import {default as PunchOutVonKaiser} from "Arts/Images/PunchOut-VonKaiser.png";
import {default as PunchOutSuperMachoMan} from "Arts/Images/PunchOut-SuperMachoMan.png";
import {default as PunchOutMrSandman} from "Arts/Images/PunchOut-MrSandman.png";
import {default as PunchOutMikeTyson} from "Arts/Images/PunchOut-MikeTyson.png";
import {default as PunchOutPortraits} from "Arts/Images/PunchOut-Portraits.png";
import {default as PunchOutFighters} from "Arts/Images/PunchOut-Fighters.png";
import {default as PunchOutVictoryLap} from "Arts/Images/PunchOut-VictoryLap.png";
import {default as SunsetRidersWantedCharacters} from "Arts/Images/SunsetRiders-WantedCharacters.png";
import {default as SuperMarioWorldWorldMap} from "Arts/Images/SuperMarioWorld-WorldMap.png";
import {default as TMNTMoods} from "Arts/Images/TMNT-Moods.png";
import {default as Airbender} from "Arts/Images/Airbender.png";
import {default as MegaManXHerosJourney} from "Arts/Images/MegaManX-HerosJourney.png";
import {default as MegaManXApprentice} from "Arts/Images/MegaManX-Apprentice.png";
import {default as ManiacMansionTwentyYearsAgo} from "Arts/Images/ManiacMansion-TwentyYearsAgo.png";
import {default as KirbysAdventureManySidesOfKirby} from "Arts/Images/KirbysAdventure-ManySidesOfKirby.png";
import {default as FestersQuestChillin} from "Arts/Images/FestersQuest-Chillin.png";
import {default as SunsetRidersBuryMeWithMyMoney} from "Arts/Images/SunsetRiders-BuryMeWithMyMoney.png";
import {default as DuckHunt} from "Arts/Images/DuckHunt.png";

export interface ArtMetadata {
	Title: string;
	FullPath: string;
	ThumbnailPath: string;
}

export class ArtService {
	constructor() {
		this.SelectedArtMetadata = new Observable<ArtMetadata|null>(null);
	}

	public SelectedArtMetadata: Observable<ArtMetadata|null>;

	public AllArtByCategory : Dictionary<ArtMetadata[]> = {
		"Everything": [
			{ Title: "20 Years Ago", FullPath: ManiacMansionTwentyYearsAgo, ThumbnailPath: ManiacMansionTwentyYearsAgo },
			{ Title: "On the Hunt", FullPath: DuckHunt, ThumbnailPath: DuckHunt },
			{ Title: "Just Chillin", FullPath: FestersQuestChillin, ThumbnailPath: FestersQuestChillin },
			{ Title: "Many Sides of Kirby", FullPath: KirbysAdventureManySidesOfKirby, ThumbnailPath: KirbysAdventureManySidesOfKirby },
			{ Title: "Moods", FullPath: TMNTMoods, ThumbnailPath: TMNTMoods },
			{ Title: "Airbender", FullPath: Airbender, ThumbnailPath: Airbender },
			{ Title: "Bury Me With My Money", FullPath: SunsetRidersBuryMeWithMyMoney, ThumbnailPath: SunsetRidersBuryMeWithMyMoney },
			{ Title: "Wanted", FullPath: SunsetRidersWantedCharacters, ThumbnailPath: SunsetRidersWantedCharacters },
			{ Title: "World Map", FullPath: SuperMarioWorldWorldMap, ThumbnailPath: SuperMarioWorldWorldMap },
			{ Title: "Hero's Journey", FullPath: MegaManXHerosJourney, ThumbnailPath: MegaManXHerosJourney },
			{ Title: "Apprentice", FullPath: MegaManXApprentice, ThumbnailPath: MegaManXApprentice },
			{ Title: "Glass Joe", FullPath: PunchOutGlassJoe, ThumbnailPath: PunchOutGlassJoe },
			{ Title: "Von Kaiser", FullPath: PunchOutVonKaiser, ThumbnailPath: PunchOutVonKaiser },
			{ Title: "Piston Honda", FullPath: PunchOutPistonHonda, ThumbnailPath: PunchOutPistonHonda },
			{ Title: "Don Flamenco", FullPath: PunchOutDonFlamenco, ThumbnailPath: PunchOutDonFlamenco },
			{ Title: "King Hippo", FullPath: PunchOutKingHippo, ThumbnailPath: PunchOutKingHippo },
			{ Title: "Great Tiger", FullPath: PunchOutGreatTiger, ThumbnailPath: PunchOutGreatTiger },
			{ Title: "Bald Bull", FullPath: PunchOutBaldBull, ThumbnailPath: PunchOutBaldBull },
			{ Title: "Soda Popinski", FullPath: PunchOutSodaPopinski, ThumbnailPath: PunchOutSodaPopinski },
			{ Title: "Mr. Sandman", FullPath: PunchOutMrSandman, ThumbnailPath: PunchOutMrSandman },
			{ Title: "Super Macho Man", FullPath: PunchOutSuperMachoMan, ThumbnailPath: PunchOutSuperMachoMan },
			{ Title: "Mike Tyson", FullPath: PunchOutMikeTyson, ThumbnailPath: PunchOutMikeTyson },
			{ Title: "Portraits", FullPath: PunchOutPortraits, ThumbnailPath: PunchOutPortraits },
			{ Title: "Lineup", FullPath: PunchOutFighters, ThumbnailPath: PunchOutFighters },
			{ Title: "Victory Lap", FullPath: PunchOutVictoryLap, ThumbnailPath: PunchOutVictoryLap },
		],
	};

	public static Instance: ArtService = new ArtService();
}
