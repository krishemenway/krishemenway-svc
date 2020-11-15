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
import {default as PunchOutVictoryLap} from "Arts/Images/PunchOut-VictoryLap.png";
import {default as SunsetRidersWantedCharacters} from "Arts/Images/SunsetRiders-WantedCharacters.png";
import {default as TMNTMoods} from "Arts/Images/TMNT-Moods.png";
import {default as Airbender} from "Arts/Images/Airbender.png";

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
			{ Title: "Moods", FullPath: TMNTMoods, ThumbnailPath: TMNTMoods },
			{ Title: "Airbender", FullPath: Airbender, ThumbnailPath: Airbender },
			{ Title: "Wanted", FullPath: SunsetRidersWantedCharacters, ThumbnailPath: SunsetRidersWantedCharacters },
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
			{ Title: "Victory Lap", FullPath: PunchOutVictoryLap, ThumbnailPath: PunchOutVictoryLap },
		],
	};

	public static Instance: ArtService = new ArtService();
}
