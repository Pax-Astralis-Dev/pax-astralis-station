using System.Linq;
using Content.Shared._NF.BountyContracts;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client._NF.BountyContracts.UI;

[GenerateTypedNameReferences]
public sealed partial class BountyContractUiFragmentCreate : Control
{
    public event Action<BountyContractRequest>? OnCreatePressed;
    public event Action? OnCancelPressed;

    private List<BountyContractTargetInfo> _targets = new();
    private List<string> _vessels = new();

    public BountyContractUiFragmentCreate()
    {
        RobustXamlLoader.Load(this);
        NameSelector.OnItemSelected += (opt) => OnNameSelected(opt.Id);
        VeselSelector.OnItemSelected += (opt) => OnVesselSelected(opt.Id);

        CustomNameButton.OnToggled += OnCustomNameToggle;
        CustomVeselButton.OnToggled += OnCustomVeselToggle;
        NameEdit.OnTextChanged += NameEditOnTextChanged;
        RewardEdit.OnTextChanged += OnPriceChanged;

        var descPlaceholder = Loc.GetString("bounty-contracts-ui-create-description-placeholder");
        DescriptionEdit.Placeholder = new Rope.Leaf(descPlaceholder);
        RewardEdit.Text = SharedBountyContractSystem.MinimalReward.ToString();

        CreateButton.OnPressed += _ => OnCreatePressed?.Invoke(GetBountyContract());
        CancelButton.OnPressed += _ => OnCancelPressed?.Invoke();

        UpdateDisclaimer();
    }

    private void OnPriceChanged(LineEdit.LineEditEventArgs obj)
    {
        UpdateDisclaimer();
    }

    private void NameEditOnTextChanged(LineEdit.LineEditEventArgs obj)
    {
        UpdateDisclaimer();
    }

    public void SetPossibleTargets(List<BountyContractTargetInfo> targets)
    {
        // make sure that all targets sorted by names alphabetically
        _targets = targets.OrderBy(target => target.Name).ToList();

        // update names dropdown
        NameSelector.Clear();
        for (var i = 0; i < _targets.Count; i++)
        {
            NameSelector.AddItem(_targets[i].Name, i);
        }

        // set selector to first option
        OnNameSelected(0);
    }

    public void SetVessels(List<string> vessels)
    {
        // make sure that all ships sorted by names alphabetically
        vessels.Sort();

        // add unknown option as a first option
        vessels.Insert(0, Loc.GetString("bounty-contracts-ui-create-vessel-unknown"));
        _vessels = vessels;

        // update ships dropdown
        VeselSelector.Clear();
        for (var i = 0; i < _vessels.Count; i++)
        {
            VeselSelector.AddItem(_vessels[i], i);
        }

        // set vessel to unknown
        OnVesselSelected(0);
    }

    private void UpdateDna(string? dnaStr)
    {
        DnaLabel.Text = dnaStr ?? Loc.GetString("bounty-contracts-ui-create-dna-unknown");
    }

    private void OnNameSelected(int itemIndex)
    {
        if (itemIndex >= _targets.Count)
            return;

        NameSelector.SelectId(itemIndex);

        // update dna
        var selectedTarget = _targets[itemIndex];
        var dnaStr = selectedTarget.DNA;
        UpdateDna(dnaStr);

        UpdateDisclaimer();
    }

    private void OnVesselSelected(int itemIndex)
    {
        if (itemIndex >= _vessels.Count)
            return;

        VeselSelector.SelectId(itemIndex);
    }

    private void OnCustomNameToggle(BaseButton.ButtonToggledEventArgs customToggle)
    {
        NameSelector.Visible = !customToggle.Pressed;
        NameEdit.Visible = customToggle.Pressed;

        UpdateDna(GetTargetDna());
        UpdateDisclaimer();
    }

    private void OnCustomVeselToggle(BaseButton.ButtonToggledEventArgs customToggle)
    {
        VeselSelector.Visible = !customToggle.Pressed;
        VeselEdit.Visible = customToggle.Pressed;

        OnVesselSelected(0);
    }

    private void UpdateDisclaimer()
    {
        // check if reward is valid
        var reward = GetReward();
        if (reward < SharedBountyContractSystem.MinimalReward)
        {
            var err = Loc.GetString("bounty-contracts-ui-create-error-too-cheap",
                ("reward", SharedBountyContractSystem.MinimalReward));
            DisclaimerLabel.SetMessage(err);
            CreateButton.Disabled = true;
            return;
        }

        // check if name is valid
        var name = GetTargetName();
        if (name == "")
        {
            var err = Loc.GetString("bounty-contracts-ui-create-error-no-name");
            DisclaimerLabel.SetMessage(err);
            CreateButton.Disabled = true;
            return;
        }

        // all looks good
        DisclaimerLabel.SetMessage(Loc.GetString("bounty-contracts-ui-create-ready"));
        CreateButton.Disabled = false;
    }

    public int GetReward()
    {
        var priceStr = RewardEdit.Text;
        return int.TryParse(priceStr, out var price) ? price : 0;
    }

    public BountyContractTargetInfo? GetTargetInfo()
    {
        BountyContractTargetInfo? info = null;
        if (!CustomNameButton.Pressed)
        {
            var id = NameSelector.SelectedId;
            if (id < _targets.Count)
                info = _targets[id];
        }
        else
        {
            info = new BountyContractTargetInfo
            {
                Name = NameEdit.Text,
                DNA = null
            };
        }

        return info;
    }

    public string GetTargetName()
    {
        var info = GetTargetInfo();
        return info != null ? info.Value.Name : "";
    }

    public string? GetTargetDna()
    {
        var info = GetTargetInfo();
        return info?.DNA;
    }

    public string GetVessel()
    {
        var vessel = "";

        if (!CustomVeselButton.Pressed)
        {
            var id = VeselSelector.SelectedId;
            if (id < _vessels.Count)
                vessel = _vessels[id];
        }
        else
        {
            vessel = VeselEdit.Text;
        }

        return vessel;
    }

    public BountyContractRequest GetBountyContract()
    {
        var info = new BountyContractRequest
        {
            Name = GetTargetName(),
            DNA = GetTargetDna(),
            Vessel = GetVessel(),
            Description = Rope.Collapse(DescriptionEdit.TextRope),
            Reward = GetReward()
        };
        return info;
    }
}