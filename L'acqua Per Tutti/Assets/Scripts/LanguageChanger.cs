using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{   
    public GameObject app;
    public TextMeshProUGUI welcome_text1, welcome_text2, welcome_text3, welcome_text4, welcome_text5, calibration_text;
    //------layer1-----------
    public TextMeshProUGUI screenCanvas_layer1_title, screenCanvas_layer1_button1, screenCanvas_layer1_button2, screenCanvas_layer1_button3, screenCanvas_layer1_button4;
    //ground
    public TextMeshProUGUI worldCanvas_layer1_ground_text;
    public Image worldCanvas_layer1_ground_image;
    public Sprite worldCanvas_layer1_ground_image_ita, worldCanvas_layer1_ground_image_eng;
    //surface
    public TextMeshProUGUI worldCanvas_layer1_surface_text;
    //sea
    public TextMeshProUGUI worldCanvas_layer1_sea_text;
    public Image worldCanvas_layer1_sea_image;
    public Sprite worldCanvas_layer1_sea_image_ita, worldCanvas_layer1_sea_image_eng;
    //reused
    public TextMeshProUGUI worldCanvas_layer1_reused_text;
    public Image worldCanvas_layer1_reused_image;
    public Sprite worldCanvas_layer1_reused_image_ita, worldCanvas_layer1_reused_image_eng;
    public GameObject reusedObject;


    //------layer2-------------
    public TextMeshProUGUI screenCanvas_layer2_title, screenCanvas_layer2_button1, screenCanvas_layer2_button2, screenCanvas_layer2_button3;
    //urban
    public TextMeshProUGUI worldCanvas_layer2_urban_text;
    public Image worldCanvas_layer2_urban_image;
    public Sprite worldCanvas_layer2_urban_image_ita, worldCanvas_layer2_urban_image_eng;
    //agriculture
    public TextMeshProUGUI worldCanvas_layer2_agriculture_text;
    public Image worldCanvas_layer2_agriculture_image;
    public Sprite worldCanvas_layer2_agriculture_image_ita, worldCanvas_layer2_agriculture_image_eng;
    //industry
    public TextMeshProUGUI worldCanvas_layer2_industry_text;
    public Image worldCanvas_layer2_industry_image;
    public Sprite worldCanvas_layer2_industry_image_ita, worldCanvas_layer2_industry_image_eng;

    //-----------layer3---------------
    public TextMeshProUGUI screenCanvas_layer3_title;
    public GameObject reusedObject_layer3;
    public TextMeshProUGUI worldCanvas_layer3_text;
    public Image worldCanvas_layer3_image;
    public Sprite worldCanvas_layer3_image_ita, worldCanvas_layer3_image_eng;


    //------layer4-----------
    public TextMeshProUGUI screenCanvas_layer4_title, screenCanvas_layer4_button1, screenCanvas_layer4_button2, screenCanvas_layer4_button3;
    //P1
    public GameObject P1_Object;
    public TextMeshProUGUI worldCanvas_layer4_P1_text;
    //P2
    public GameObject P2_Object;
    public TextMeshProUGUI worldCanvas_layer4_P2_text;
    //P3
    public TextMeshProUGUI worldCanvas_layer4_P3_text;

    public void changetoEnglish_welcomePanel(){
        welcome_text1.text = "Welcome to Biblioteca Comunale Bottini dell'Olio!";
        welcome_text2.text = "This is an interactive experience to show water symbiosis in Cecina and Rosignano.\n\nTo begin please follow the instructions on the next screen.";
        welcome_text3.text = "Welcome to Biblioteca Comunale Bottini dell'Olio!";
        welcome_text4.text = "If you wish to experience water symbiosis between Cecina and Rosignano area offsite, you can grab the Map (map.jpg) by downloading it from this link";
        welcome_text5.text = "Otherwise, you can find it installed at the Livorno library. Enjoy.";
        calibration_text.text = "Point your screen at this image";
     }

     public void changetoItalian_welcomePanel(){
        welcome_text1.text = "Benvenuti alla Biblioteca Comunale Bottini dell’Olio!";
        welcome_text2.text = "Questa è un’esperienza interattiva per mostrare la simbiosi dell’acqua a Cecina e Rosignano.\n\nPer iniziare, per favore, segui le istruzioni nella prossima schermata.";
        welcome_text3.text = "Benvenuti alla Biblioteca Comunale Bottini dell’Olio!";
        welcome_text4.text = "Se desideri vivere l'esperienza della simbiosi d'acqua tra Cecina e la zona di Rosignano, puoi ottenere la mappa (map.jpg) scaricandola da questo link";
        welcome_text5.text = "In caso contrario, la troverai installata presso la biblioteca di Livorno. Buon divertimento.";
        calibration_text.text = "Inquadra l’ immagine";
     }

    public void changetoItalian(){
        if(app.transform.GetComponent<Calibration>().calibrationOn){
            changetoItalian_welcomePanel();
        }else{
            //-----layer 1-------------
            screenCanvas_layer1_title.text = "RISORSE D'ACQUA";
            screenCanvas_layer1_button1.text = "ACQUA DI FALDA";
            screenCanvas_layer1_button2.text = "ACQUE SUPERFICIALI";
            screenCanvas_layer1_button3.text = "ACQUA DI MARE";
            screenCanvas_layer1_button4.text = "ACQUA DI RIUSO";

            //ground
            worldCanvas_layer1_ground_text.text = "Nella mappa vengono indicate i pozzi di acqua potabile presenti nell’area di Cecina e Rosignano. Questi pozzi sono gestiti da ASA, gestore del servizio idrico integrato, la quale effettua tutti i trattamenti necessari per rendere l’acqua potabile. Quest’acqua viene principalmente fornita alle abitazioni e alle attività della zona.";
            worldCanvas_layer1_ground_image.sprite = worldCanvas_layer1_ground_image_ita;
            //surface
            worldCanvas_layer1_surface_text.text = "Le acque superficiali sono composte, oltre che dal fiume Fine, da laghi artificiali creati e gestiti da Solvay per avere riserve di acqua anche nei periodi di maggiore siccità. Questi laghi si riempiono con le piogge invernali e l’acqua raccolta viene utilizzata nel periodo estivo da Solvay per scopi industriali o dalle aziende agricole per l’irrigazione dei campi. Queste riserve d’acqua svolgono un ruolo fondamentale nella gestione della risorsa idrica del territorio nel periodo estivo, permettendo di salvaguardare l’acqua di falda.";
            //sea
            worldCanvas_layer1_sea_text.text = "L'acqua di mare viene utilizzata esclusivamente da Solvay.\nSolvay preleva l’acqua direttamente dal mare aperto e la utilizza per diversi scopi all'interno del sito industriale.";
            worldCanvas_layer1_sea_image.sprite = worldCanvas_layer1_sea_image_ita;
            //reused
            worldCanvas_layer1_reused_text.text = "L'acqua di riuso è prodotta dal Consorzio ARETUSA, che tratta le acque uscenti dai depuratori di Cecina e Rosignano per consentirne un ulteriore riutilizzo nell'industria Solvay.\n\nIl trattamento effettuato dal consorzio è indispensabile per rendere le acque reflue urbane nuovamente riutilizzabili sia per l’industria che per l’agricoltura.";
            worldCanvas_layer1_reused_image.sprite = worldCanvas_layer1_reused_image_ita;
                //cecina 1, rosignano 2, aretusa 0
            reusedObject.transform.Find("ReusedObject0").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "IMPIANTO AFFINAMENTO ARETUSA";
            reusedObject.transform.Find("ReusedObject1").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE CECINA";
            reusedObject.transform.Find("ReusedObject2").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE ROSIGNANO";

            //-----layer 2-------------
            screenCanvas_layer2_title.text = "UTENZE";
            screenCanvas_layer2_button1.text = "USO URBANO";
            screenCanvas_layer2_button2.text = "USO IN AGRICOLTURA";
            screenCanvas_layer2_button3.text = "USO INDUSTRIALE";

            //urban
            worldCanvas_layer2_urban_text.text = "Le acque destinate all’utilizzo urbano si dividono principalmente in:\n\n•    Acque per uso domestico: sono presenti sul territorio di Cecina e Rosignano circa 25’500 utenze\n•    Acque per attività commerciali: sono presenti sul territorio di Cecina e Rosignano circa 1’800 utenze";
            worldCanvas_layer2_urban_image.sprite = worldCanvas_layer2_urban_image_ita;
            //agriculture
            worldCanvas_layer2_agriculture_text.text = "Le acque destinate ad utilizzo agricolo si dividono principalmente in:\n\n•    Acque di irrigazione: sono presenti sul territorio di Cecina e Rosignano 45 utenze\n•    Acque utilizzate da aziende agricole: sono presenti sul territorio di Cecina e Rosignano circa 10 utenze";
            worldCanvas_layer2_agriculture_image.sprite = worldCanvas_layer2_agriculture_image_ita;
            //industry
            worldCanvas_layer2_industry_text.text = "Le acque destinate ad utilizzo industriale sono associate alle attività industriali presenti a Cecina e Rosignano, in particolare si individuano:\n\n•    Circa 70 utenze industriali a Cecina\n•    Circa 60 utenze industriali a Rosignano (considerando come unica utenza la contrada Solvay)";
            worldCanvas_layer2_industry_image.sprite = worldCanvas_layer2_industry_image_ita;

            //-----layer 3-------------
            screenCanvas_layer3_title.text = "CONNESSIONE SIMBIOTICA ACQUA";
            reusedObject_layer3.transform.Find("ReusedObject0").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "IMPIANTO AFFINAMENTO ARETUSA";
            reusedObject_layer3.transform.Find("ReusedObject1").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE CECINA";
            reusedObject_layer3.transform.Find("ReusedObject2").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE ROSIGNANO";
            reusedObject_layer3.transform.Find("ReusedObject4").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "MUNICIPALIZZATA ROSIGNANO";
            reusedObject_layer3.transform.Find("ReusedObject5").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "MUNICIPALIZZATA CECINA";

            worldCanvas_layer3_text.text = "La simbiosi industriale del Consorzio Aretusa consente di salvaguardare e proteggere la risorsa idrica. Il Consorzio Aretusa, costituito dalle società ASA Azienda Servizi Ambientali Spa, Solvay Chimica Italia spa e Termomeccanica Ecologia Spa, nasce nel 2001 per la realizzazione dell’impianto di post-trattamento Aretusa. L’impianto di riciclo e riuso delle acque reflue Aretusa, è alimentato dai depuratori di Rosignano e Cecina mediante condotte dedicate e produce reflui trattati inviandoli agli adiacenti impianti dello stabilimento chimico Solvay, dove sono utilizzati a scopi industriali.\nIn questo modo si evita lo scarico delle acque depurate e si riduce il consumo di acqua di falda.";
            worldCanvas_layer3_image.sprite = worldCanvas_layer3_image_ita;


            //-----------layer4---------------
            screenCanvas_layer4_title.text = "INNOVAZIONI ULTIMATE";
            screenCanvas_layer4_button1.text = "INNOVAZIONI ULTIMATE P1";
            screenCanvas_layer4_button2.text = "INNOVAZIONI ULTIMATE P2";
            screenCanvas_layer4_button3.text = "INNOVAZIONI ULTIMATE P3";
            //P1
            worldCanvas_layer4_P1_text.text = "Cecina e Rosignano sono due comuni con un’importante porzione di sviluppo lungo la costa. Per tale motivo si assiste frequentemente a fenomeni di intrusione salina all’interno dei sistemi fognari. Tale condizione può comportare un’alterazione della qualità dell’acqua, compromettendo il riutilizzo industriale all’interno della simbiosi industriale di Aretusa.\n\nTramite il progetto H2020 ULTIMATE sono stati, quindi, installati degli strumenti per la misura della portata e della conducibilità in punti chiave delle due reti fognarie, che fungono da sentinelle per segnalare innalzamenti della salinità nell’acqua. In questo modo si cerca di prevedere la qualità delle acque provenienti da Cecina e Rosignano in modo da poter gestire i due flussi tramite un sistema di miscelazione intelligente che consenta di minimizzare la salinità in ingresso ad Aretusa.";
            P1_Object.transform.Find("P10").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "IMPIANTO AFFINAMENTO ARETUSA";
            P1_Object.transform.Find("P11").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE CECINA";
            P1_Object.transform.Find("P12").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEPURATORE ROSIGNANO";
            P1_Object.transform.Find("Legend").transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Misuatori di portata";
            P1_Object.transform.Find("Legend").transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sensori di conducibilità";
            //P2
            worldCanvas_layer4_P2_text.text = "La simbiosi industriale di Aretusa nasce con l’obiettivo di consentire il riutilizzo industriale delle acque reflue. Al fine di massimizzare le possibilità di riutilizzo, sia industriale, che agricolo è necessario individuare trattamenti che consentano di incrementarne la qualità delle acque reflue fino al rispetto dei limiti imposti dalle normative e dalle necessità industriali.\n\nNello specifico contesto dell’impianto Aretusa, tramite il progetto H2020 ULTIMATE sono stati, quindi, testati sia in scala laboratorio che in scala pilota, diversi processi innovativi per il trattamento delle acque. Tali trattamenti hanno previsto anche il riutilizzo di sottoprodotti industriali al fine di incrementare ulteriormente la circolarità della simbiosi industriale di Aretusa.";
            P2_Object.transform.Find("P21").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "IMPIANTO AFFINAMENTO ARETUSA";
            P2_Object.transform.Find("P22").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Pilota AOP";
            P2_Object.transform.Find("P23").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Pilota addolcimento e chiariflocculazione";
            P2_Object.transform.Find("P24").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Pilota adsorbimento";
            P2_Object.transform.Find("P25").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Pilota osmosi inversa";
            //P3
            worldCanvas_layer4_P3_text.text = "Sistemi di economia circolare, come la simbiosi industriale, richiedono l’instaurarsi di relazioni tra gli stakeholders del territorio per generare una rete locale che permetta lo scambio di conoscenze, informazioni e crei sinergie  territoriali tali per cui si abbia un effettivo progresso in merito alla salvaguardia della risorsa idrica.\n\nTramite il progetto H2020 ULTIMATE si sono organizzati diversi eventi al fine di favorire lo sviluppo di tali interconnessioni. In particolare diversi incontri (Community of Practice o Living Labs) hanno consentito di identificare e discutere le principali problematiche del territorio legate alla legislazione e all’attuazione delle politiche di riuso. Inoltre si sono effettuate attività di sensibilizzazione, ad esempio anche presso le scuole, al fine di coinvolgere nelle discussioni non solo i «tecnici», ma anche la popolazione territorialmente interessata.";
        }
    }

    public void changetoEnglish(){
        if(app.transform.GetComponent<Calibration>().calibrationOn){
            changetoEnglish_welcomePanel();
        }else{
            //-------------layer1-------------------
            screenCanvas_layer1_title.text = "WATER SOURCES";
            screenCanvas_layer1_button1.text = "GROUND WATER";
            screenCanvas_layer1_button2.text = "SURFACE WATER";
            screenCanvas_layer1_button3.text = "SEA WATER";
            screenCanvas_layer1_button4.text = "REUSED WATER";

            //ground
            worldCanvas_layer1_ground_text.text = "In the map are indicated the drinking water wells in Cecina and Rosignano area. These wells are managed by the water utility “ASA”, which implements a treatment process to make this water suitable for human use. These wells are mainly used to supply fresh water to citizens and local businesses.";
            worldCanvas_layer1_ground_image.sprite = worldCanvas_layer1_ground_image_eng;
            //surface
            worldCanvas_layer1_surface_text.text = "The surface waters include Fine river and artificial lakes created and managed by Solvay to have water reserves even in periods of greater drought. These lakes act as large water reservoirs, which are refilled during the winter season by the rain and the water collected is used during the summer by Solvay for industrial purposes or by farms for field irrigation. These water reservoirs play a fundamental role in the management of water resources during the summer period, avoiding the excessive consumption of groundwater.";
            //sea
            worldCanvas_layer1_sea_text.text = "The seawater is used exclusively by Solvay.\nSolvay withdraws the seawater directly from the open sea and uses it for different purpose within the industrial site.";
            worldCanvas_layer1_sea_image.sprite = worldCanvas_layer1_sea_image_eng;
            //reused
            worldCanvas_layer1_reused_text.text = "Reuse water is produced by the Consorzio ARETUSA, which treats the wastewater produced by Cecina and Rosignano to allow further reuse in Solvay industry.\n\nThe treatment carried out by the consortium is essential to make the urban wastewater reusable for industrial and agricultural purposes.";
            worldCanvas_layer1_reused_image.sprite = worldCanvas_layer1_reused_image_eng;
                //cecina 1, rosignano 2, aretusa 0
            reusedObject.transform.Find("ReusedObject0").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ARETUSA WASTEWATER RECLAMATION PLANT";
            reusedObject.transform.Find("ReusedObject1").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "CECINA WASTEWATER TREATMENT PLANT";
            reusedObject.transform.Find("ReusedObject2").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ROSIGNANO WASTEWATER TREATMENT PLANT";

            //-----layer 2-------------
            screenCanvas_layer2_title.text = "WATER USERS";
            screenCanvas_layer2_button1.text = "URBAN   USERS";
            screenCanvas_layer2_button2.text = "AGRICULTURAL USERS";
            screenCanvas_layer2_button3.text = "INDUSTRIAL USERS";

            //urban
            worldCanvas_layer2_urban_text.text = "Water for urban use is mainly divided into:\n\n•    Water for domestic use: there are around 25500 users in the Cecina and Rosignano area\n•    Water for commercial activities: there are around 1800 users in the Cecina and Rosignano area";
            worldCanvas_layer2_urban_image.sprite = worldCanvas_layer2_urban_image_eng;
            //agriculture
            worldCanvas_layer2_agriculture_text.text = "Water for agricultural use is mainly divided into:\n\n•    Irrigation water: there are 45 users in the Cecina and Rosignano area\n•    Water used by farms: there are about 10 users in the Cecina and Rosignano area";
            worldCanvas_layer2_agriculture_image.sprite = worldCanvas_layer2_agriculture_image_eng;
            //industry
            worldCanvas_layer2_industry_text.text = "The waters for industrial use are those used by the industrial activities present in Cecina and Rosignano, in particular there are:\n\n•    About 70 industrial users in Cecina\n•    About 60 industrial users in Rosignano (considering the Solvay district as only one user)";
            worldCanvas_layer2_industry_image.sprite = worldCanvas_layer2_industry_image_eng;


            //-----layer 3-------------
            screenCanvas_layer3_title.text = "WATER SYMBIOTIC CONNECTION";
            reusedObject_layer3.transform.Find("ReusedObject0").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ARETUSA WASTEWATER RECLAMATION PLANT";
            reusedObject_layer3.transform.Find("ReusedObject1").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "CECINA WASTEWATER TREATMENT PLANT";
            reusedObject_layer3.transform.Find("ReusedObject2").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ROSIGNANO WASTEWATER TREATMENT PLANT";
            reusedObject_layer3.transform.Find("ReusedObject4").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ROSIGNANO MUNICIPALITY";
            reusedObject_layer3.transform.Find("ReusedObject5").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "CECINA MUNICIPALITY";

            worldCanvas_layer3_text.text = "The industrial symbiosis of the Consorzio Aretusa makes it possible to protect and safeguard the water resources. The Consorzio Aretusa, formed by the companies ASA Azienda Servizi Ambientali Spa, Solvay Chimica Italia spa and Termomeccanica Ecologia Spa, was founded in 2001 for the construction of the Aretusa post-treatment plant. The Aretusa wastewater recycling and reuse plant is fed by the WWTPs of Rosignano and Cecina through dedicated pipelines and produces treated wastewater which are sent to the adjacent Solvay chemical plant, where it is used for industrial purposes.\nIn this way, the discharge of treated water is avoided, and the consumption of groundwater is reduced.";
            worldCanvas_layer3_image.sprite = worldCanvas_layer3_image_eng;

            //-----------layer4---------------
            screenCanvas_layer4_title.text = "ULTIMATE INNOVATIONS";
            screenCanvas_layer4_button1.text = "ULTIMATE INNOVATIONS P1";
            screenCanvas_layer4_button2.text = "ULTIMATE INNOVATIONS P2";
            screenCanvas_layer4_button3.text = "ULTIMATE INNOVATIONS P3";
            //P1
            worldCanvas_layer4_P1_text.text = "Cecina and Rosignano are two municipalities with an important development along the coast. For this reason, we frequently witness saline intrusion phenomena within the sewage systems. This condition can lead to an alteration of the water quality, compromising industrial reuse within the industrial symbiosis of Aretusa.\n\nThrough the H2020 ULTIMATE project, instruments were therefore installed to measure the flowrate and conductivity in key points of the two sewer networks, which act as sentinels to signal increases in salinity in the water. In this way we try to predict the quality of the water coming from Cecina and Rosignano in order to be able to manage the two flows through an intelligent mixing system that allows minimizing the salinity entering Aretusa.";
            P1_Object.transform.Find("P10").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ARETUSA WASTEWATER RECLAMATION PLANT";
            P1_Object.transform.Find("P11").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "CECINA WASTEWATER TREATMENT PLANT";
            P1_Object.transform.Find("P12").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ROSIGNANO WASTEWATER TREATMENT PLANT";
            P1_Object.transform.Find("Legend").transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Flowmeter";
            P1_Object.transform.Find("Legend").transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Conductivity meter";
            //P2
            worldCanvas_layer4_P2_text.text = "The industrial symbiosis of Aretusa was born with the aim of allowing the industrial reuse of wastewater. In order to maximize the possibilities of both industrial and agricultural reuse, it is necessary to identify treatments that allow an increase in the quality of the wastewater up to compliance with the limits imposed by the regulations and industrial needs.\n\nIn the specific context of the Aretusa plant, through the H2020 ULTIMATE project, various innovative processes for water treatment were therefore tested both on a laboratory scale and on a pilot scale. These treatments also envisaged the reuse of industrial by-products in order to further increase the circularity of the industrial symbiosis of Aretusa.";
            P2_Object.transform.Find("P21").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ARETUSA WASTEWATER RECLAMATION PLANT";
            P2_Object.transform.Find("P22").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "AOP pilot plant";
            P2_Object.transform.Find("P23").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Softening & clariflocculation pilot plant";
            P2_Object.transform.Find("P24").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Adsorption pilot plant";
            P2_Object.transform.Find("P25").transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "RO pilot plant";
            //P3
            worldCanvas_layer4_P3_text.text = "The industrial symbiosis of Aretusa was born with the aim of allowing the industrial reuse of wastewater. In order to maximize the possibilities of both industrial and agricultural reuse, it is necessary to identify treatments that allow an increase in the quality of the wastewater up to compliance with the limits imposed by the regulations and industrial needs.\n\nIn the specific context of the Aretusa plant, through the H2020 ULTIMATE project, various innovative processes for water treatment were therefore tested both on a laboratory scale and on a pilot scale. These treatments also envisaged the reuse of industrial by-products in order to further increase the circularity of the industrial symbiosis of Aretusa.";
        }
    }
}
